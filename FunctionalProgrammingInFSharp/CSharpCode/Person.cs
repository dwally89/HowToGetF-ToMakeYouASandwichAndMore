

public class Person
{
    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public int Age { get; }

    public string Name { get; }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        var other = (Person)obj;
        return Age == other.Age && string.Equals(Name, other.Name);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (Age * 397) ^ (Name?.GetHashCode() ?? 0);
        }
    }
}