module TypeProviders3

open FSharp.Data

[<Literal>]
let compileTimeConnectionString = "Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=SchoolDatabase;Integrated Security=SSPI;"

let prodConnectionString = "Data Source=PRDDB;Initial Catalog=SchoolDatabase;Integrated Security=SSPI;"

type StudentClasses = SqlCommandProvider<"SELECT s.Id AS StudentId, s.FirstName, s.LastName, c.Id AS ClassId, c.Name ClassName
                                          FROM dbo.Students s
                                          JOIN dbo.StudentClasses sc on s.Id=sc.StudentId
                                          JOIN dbo.Classes c on c.Id=sc.ClassId", compileTimeConnectionString>

let ``get students taking more than three classes``() = 
    use conn = new StudentClasses(prodConnectionString)
    conn.Execute()
    |> Seq.groupBy (fun sc -> (sc.StudentId, sc.FirstName, sc.LastName))
    |> Seq.filter (fun (_, classes) -> classes |> Seq.length > 3)
    |> Seq.iter (fun ((_, firstName, lastName), classes) -> printfn "%s %s has %i classes" firstName lastName (classes |> Seq.length))