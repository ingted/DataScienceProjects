#I "../packages/FSharp.Data.2.1.0/lib/net40"
#r "FSharp.Data.dll"

open FSharp.Data

let deliveryData = CsvProvider<"delivery_data.csv">.GetSample()

let grouped = [for row in deliveryData.Rows do
                yield row.Pickup_zipcode]
                    |> Seq.countBy id 
                    |> Seq.filter(fun (z, c) -> c > 20 && z.HasValue) 
                    |> Seq.map(fun (z, c) -> z.Value)



let getItemsLessThan60ByZip zip = deliveryData.Rows 
                                    |> Seq.filter(fun i -> i.Pickup_zipcode.HasValue && i.Pickup_zipcode.Value = zip && i.Purchase_price < 60m)

let getItemsBetween60And120ByZip zip = deliveryData.Rows
                                        |> Seq.filter(fun i -> i.Pickup_zipcode.HasValue && i.Pickup_zipcode.Value = zip && (i.Purchase_price > 60m && i.Purchase_price < 120m))

let getItemsGreaterThan120ByZip zip = deliveryData.Rows
                                        |> Seq.filter(fun i -> i.Pickup_zipcode.HasValue && i.Pickup_zipcode.Value = zip && i.Purchase_price >= 120m)