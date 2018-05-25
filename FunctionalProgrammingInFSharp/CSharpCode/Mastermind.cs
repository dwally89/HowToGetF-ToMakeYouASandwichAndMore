

using System;
using System.Collections.Generic;
using System.Linq;

namespace MastermindCSharp
{
    internal class Some<T> : Option<T>
    {
        private readonly T value;

        public Some(T value)
        {
            this.value = value;
        }

        public T Value => value;

        public override TResult AcceptVisitor<TResult>(Func<Some<T>, TResult> someVisitor, Func<None<T>, TResult> noneVisitor)
        {
            return someVisitor(this);
        }
    }

    internal class None<T> : Option<T>
    {
        public override TResult AcceptVisitor<TResult>(Func<Some<T>, TResult> someVisitor, Func<None<T>, TResult> noneVisitor)
        {
            return noneVisitor(this);
        }
    }

    internal abstract class Option<T>
    {
        public abstract TResult AcceptVisitor<TResult>(Func<Some<T>, TResult> someVisitor, Func<None<T>, TResult> noneVisitor);
    }

    internal abstract class Result<TSuccess, TFailure>
    {
        public abstract TResult AcceptVisitor<TResult>(Func<TSuccess, TResult> successVisitor, Func<TFailure, TResult> failureVisitor);
    }

    internal class ValidGuess : Result<ValidGuess, InvalidGuess>
    {
        private readonly List<MatchResult> result;

        public ValidGuess(List<MatchResult> result)
        {
            this.result = result;
        }

        public List<MatchResult> Result => result;

        public override TResult AcceptVisitor<TResult>(Func<ValidGuess, TResult> successVisitor, Func<InvalidGuess, TResult> failureVisitor)
        {
            return successVisitor(this);
        }
    }

    internal class InvalidGuess : Result<ValidGuess, InvalidGuess>
    {
        private readonly string message;

        public InvalidGuess(string message)
        {
            this.message = message;
        }

        public string Message => message;

        public override TResult AcceptVisitor<TResult>(Func<ValidGuess, TResult> successVisitor, Func<InvalidGuess, TResult> failureVisitor)
        {
            return failureVisitor(this);
        }
    }

    internal enum MatchResult
    {
        Correct,
        InWrongPlace,
        Incorrect
    }

    public class CodeComparer
    {
        internal static IEnumerable<bool> UpdateMatchedItems(int i, IEnumerable<bool> existingItems)
        {
            return existingItems.Select((existingItem, idx) => idx == i || existingItem);
        }

        private static Option<int> FindItemInWrongPlace<T>(T item, IEnumerable<bool> matchedItems, IEnumerable<T> items)
        {
            var result = items
                .Select((citem, cindex) => Tuple.Create(cindex, citem))
                .Where(tuple => !matchedItems.ElementAt(tuple.Item1) && Equals(tuple.Item2, item))
                .Select(tuple => tuple.Item1);
            return result.Any() ? (Option<int>)new Some<int>(result.First()) : new None<int>();
        }

        private static Tuple<List<MatchResult>, IEnumerable<bool>> CompareOne<T>(IEnumerable<T> code, Tuple<List<MatchResult>, IEnumerable<bool>> acc, Tuple<int, T> pair)
        {
            var existingResults = acc.Item1;
            var matchedItems = acc.Item2;
            var i = pair.Item1;
            var item = pair.Item2;
            var tuple = Equals(item, code.ElementAt(i))
                ? Tuple.Create(MatchResult.Correct, UpdateMatchedItems(i, matchedItems))
                : FindItemInWrongPlace(item, matchedItems, code)
                    .AcceptVisitor(
                    some => Tuple.Create(MatchResult.InWrongPlace, UpdateMatchedItems(some.Value, matchedItems)),
                    none => Tuple.Create(MatchResult.Incorrect, matchedItems));

            return Tuple.Create(existingResults.Concat(new List<MatchResult> { tuple.Item1 }).ToList(), tuple.Item2);
        }

        private static IEnumerable<bool> GetAllInCorrectPlace<T>(IEnumerable<T> guess, IEnumerable<T> code)
        {
            return guess.Select((item, idx) => Equals(code.ElementAt(idx), item));
        }

        internal static Result<ValidGuess, InvalidGuess> Compare<T>(IEnumerable<T> guess, IEnumerable<T> code)
        {
            var codeLength = code.Count();
            if (guess.Count() != codeLength)
            {
                return new InvalidGuess($"Incorrect guess length. Expected length: {codeLength}");
            }

            var matchedItems = GetAllInCorrectPlace(guess, code);
            return new ValidGuess(guess
                .Select((item, i) => Tuple.Create(i, item))
                .Aggregate(Tuple.Create(new List<MatchResult>(), matchedItems),
                    (acc, pair) => CompareOne(code, acc, pair))
                .Item1);
        }

        private void CompareAndPrintResult<T>(IEnumerable<T> guess, IEnumerable<T> code)
        {
            var result = Compare(guess, code);
            result.AcceptVisitor<object>(
                validGuess =>
                {
                    Console.WriteLine(
                        $"{Utils.ConvertIEnumerableToString(guess)}, {Utils.ConvertIEnumerableToString(code)}, {Utils.ConvertIEnumerableToString(validGuess.Result)}");
                    return null;
                },
                invalidGuess =>
                {
                    Console.WriteLine(
                        $"{Utils.ConvertIEnumerableToString(guess)}, {Utils.ConvertIEnumerableToString(code)}, {Utils.ConvertIEnumerableToString(invalidGuess.Message)}");
                    return null;
                });
        }
    }

    internal class Game
    {
        internal static bool CompareResults<T>(IEnumerable<T> result2, Result<ValidGuess, InvalidGuess> result1)
        {
            return result1.AcceptVisitor(
                validGuess => {
                    var m = validGuess.Result;
                    var length = m.Count;
                    return Enumerable.Range(0, length).All(i => Equals(m.ElementAt(i), result2.ElementAt(i)));
                },
                invalidGuess => false);
        }

        internal static IEnumerable<T> Start<T>(IEnumerable<T> code, IEnumerable<IEnumerable<T>> remainingGuesses)
        {
            var guess = remainingGuesses.First();
            Console.WriteLine($"Guess: {Utils.ConvertIEnumerableToString(guess)}");
            var result = CodeComparer.Compare(guess, code);
            return result.AcceptVisitor(
                validGuess => {
                    var m = validGuess.Result;
                    return m.All(r => r == MatchResult.Correct)
                        ? guess
                        : Start(code, remainingGuesses
                            .Skip(1)
                            .Where(g =>
                            {
                                var r = CodeComparer.Compare(g, guess);
                                return CompareResults(m, r);
                            }));
                },
                invalidGuess => { throw new InvalidOperationException(invalidGuess.Message); });
        }

        private static List<List<T>> GenerateGuesses<T>(int length, IEnumerable<T> options, List<List<T>> existingGuesses)
        {
            return length == 0
                ? existingGuesses
                : GenerateGuesses(length - 1, options, options
                    .SelectMany(o =>
                    {
                        return !existingGuesses.Any()
                            ? new List<List<T>> { new List<T> { o } }
                            : existingGuesses.Select(g => new List<T> { o }.Concat(g).ToList());
                    }).ToList());
        }

        internal static void PlayGame()
        {
            var options = Enumerable.Range(1, 6).ToList();
            const int length = 4;
            var random = new Random();
            var code = Enumerable.Range(1, length).Select(i => options.ElementAt(random.Next(options.Count))).ToList();
            Console.WriteLine(Utils.ConvertIEnumerableToString(code));
            var allGuesses = GenerateGuesses(length, options, new List<List<int>>());
            Console.WriteLine(Utils.ConvertIEnumerableOfIEnumerablesToString(allGuesses));
            Start(code, allGuesses);
        }
    }

    internal class Utils
    {
        internal static string ConvertIEnumerableToString<T>(IEnumerable<T> code)
        {
            return $"[{string.Join(",", code)}]";
        }

        internal static string ConvertIEnumerableOfIEnumerablesToString<T>(IEnumerable<IEnumerable<T>> allGuesses)
        {
            return string.Join("\n", allGuesses.Select(ConvertIEnumerableToString));
        }
    }
}