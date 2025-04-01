namespace TypeProvidersDemo

open FSharp.Data

module Simple =
    type SimpleJson = JsonProvider<"""{ "name": "Alice", "age": 30, "city": "New York" }""">

    let data = SimpleJson.Parse("""{ "name": "Bob", "age": 25, "city": "London" }""")
    printfn $"Bonjour. Mon nom est %s{data.Name} et j\'ai %d{data.Age} ans."
    
module Simple2 =
    [<Literal>]
    let jsonString = """
    [
      { "name": "Alice", "age": 30, "city": "New York", "salary": 65545.50 },
      { "name": "Bob", "age": 25, "city": "London", "salary": 86500 },
      { "name": "Charlie", "age": 35, "city": "Montreal" }
    ]
    """

    type Users = JsonProvider<jsonString>

    let users = Users.Parse(jsonString)
    let allNames = users |> Array.map _.Name
    let totalAges = users |> Array.sumBy _.Age
    
module SimpleWeb =
    type BookCatalog = XmlProvider<"http://www.w3schools.com/xml/books.xml">

    let catalog = BookCatalog.Load("http://www.w3schools.com/xml/books.xml")
    
    let allTitles = catalog.Books |> Array.collect _.Authors |> Array.distinct
    let averageBookPrice = catalog.Books |> Array.map _.Price |> Array.average
    
    let booByName bookTitle =
        catalog.Books |> Array.tryFind (fun b -> b.Title = bookTitle)
        
    let averageBookPrices minPrice =
        catalog.Books |> Array.map _.Price |> Array.filter (fun p -> p >= minPrice) |> Array.average
    
module Nasa =
    type MarsWeather = JsonProvider<"https://api.nasa.gov/insight_weather/?api_key=DEMO_KEY&feedtype=json&ver=1.0">
    
    let marsWeather = MarsWeather.GetSample()