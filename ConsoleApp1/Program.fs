open System
open System.Drawing
open System.Windows.Forms
open System.IO
open System.Diagnostics

// Function to generate a unique ID for each ticket
let generateTicketID () =
    "TICKET-" + (Guid.NewGuid().ToString())

let readBookingFromFile time =
    let filePath = sprintf "D:\\f#\\ConsoleApp1\\ConsoleApp1\\ticket details\\%s_bookingDetails.txt" time
    if File.Exists(filePath) then
        File.ReadAllLines(filePath)
    else
        [||]  // Return an empty array if the file does not exist


let saveBookingToFile name time selectedSeats ticketID =
    let filePath = sprintf "D:\\f#\\ConsoleApp1\\ConsoleApp1\\ticket details\\%s_bookingDetails.txt" time
    let details = sprintf "Ticket ID: %s\nName: %s\nTime: %s\nReserved Seats:\n%s\n\n" ticketID name time selectedSeats
    File.AppendAllText(filePath, details)

