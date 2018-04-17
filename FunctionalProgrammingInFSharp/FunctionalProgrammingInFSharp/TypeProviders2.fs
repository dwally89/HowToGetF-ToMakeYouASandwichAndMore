module TypeProviders2

open FSharp.Data

type EntitlementUser = JsonProvider<""" [{"aqrUserId":"wallisd","userName":"Daniel Wallis","altName":"Daniel Wallis","department":"IT","group":"Enterprise Dev","hR_Id":56368,"aqrTitle":"Analyst","isManager":false,"manager":"Vitaliy Razhanskiy","managerId":"razhanskiyv","hireDate":"2016-04-11T00:00:00","employeeType":"Employee","adpGroupNumber":"71","email":"Daniel.Wallis@aqr.com","degree":null,"firstName":"Daniel","lastName":"Wallis"}] """>    

let PrintAllUsers() = 
    let url = "http://aqrweb/entitlementsystem/api/entitlement/GetHRUsers"    
    let rawJson = Http.RequestString(url, customizeHttpRequest = fun req -> req.UseDefaultCredentials <- true
                                                                            req)    
    EntitlementUser.Parse rawJson
    |> Seq.iter (fun user -> printfn "%s - %s" user.AqrUserId user.AltName)  