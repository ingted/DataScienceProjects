#I "../packages/FSharp.Data.2.0.14/lib/net40"
#r "FSharp.Data.dll"
open FSharp.Data

let deliveryData = CsvProvider<"delivery_data.csv">.GetSample()

let validZipCodes = deliveryData.Rows
                        |> Seq.map(fun r -> r.pickup_zipcode)
                        |> Seq.countBy id
                        |> Seq.filter(fun (z, c) -> c > 20 && z.HasValue)
                        |> Seq.map(fun (z, c) -> z.Value)

let filterByZip (pickupZip: System.Nullable<int>) inputZip = pickupZip.HasValue && pickupZip.Value = inputZip

let getItemsLessThan60ByZip zip = deliveryData.Rows
                                    |> Seq.filter(fun i -> (filterByZip i.pickup_zipcode zip) && i.purchase_price < 60m)
                                    |> Seq.map(fun i -> i.pickup_zipcode)
                                    |> Seq.countBy id

let getItemsBetween60And120ByZip zip = deliveryData.Rows
                                        |> Seq.filter(fun i -> (filterByZip i.pickup_zipcode zip) && (i.purchase_price > 60m && i.purchase_price < 120m))
                                        |> Seq.map(fun i -> i.pickup_zipcode)
                                        |> Seq.countBy id

let getItemsGreaterThan120ByZip zip = deliveryData.Rows
                                        |> Seq.filter(fun i -> (filterByZip i.pickup_zipcode zip) && i.purchase_price >= 120m)
                                        |> Seq.map(fun i -> i.pickup_zipcode)
                                        |> Seq.countBy id

validZipCodes |> Seq.collect(fun i -> getItemsLessThan60ByZip i) |> Seq.toList
validZipCodes |> Seq.collect(fun i -> getItemsBetween60And120ByZip i) |> Seq.toList
validZipCodes |> Seq.collect(fun i -> getItemsGreaterThan120ByZip i) |> Seq.toList

// Need to use Deedle to now create a frame to display the data (refactor a bit of the above to completely use Deedle or just use for the frame).