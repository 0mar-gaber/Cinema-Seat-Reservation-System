open System
open System.Drawing
open System.Windows.Forms
open System.IO
open System.Diagnostics

// Generate a unique ID for each ticket
let generateTicketID () =
    "TICKET-" + (Guid.NewGuid().ToString())

// Read booking details from a file based on show time
let readBookingFromFile time =
    let filePath = sprintf "C:\\Users\\amrya\\Desktop\\ConsoleApp1\\ConsoleApp1\\ConsoleApp1\\ticket details\\%s_bookingDetails.txt" time
    if File.Exists(filePath) then
        File.ReadAllLines(filePath)
    else
        [||]

// Save booking details to a file with the corresponding show time
let saveBookingToFile name time selectedSeats ticketID =
    let filePath = sprintf "C:\\Users\\amrya\\Desktop\\ConsoleApp1\\ConsoleApp1\\ConsoleApp1\\ticket details\\%s_bookingDetails.txt" time
    let details = sprintf "Ticket ID: %s\nName: %s\nTime: %s\nReserved Seats:\n%s\n\n" ticketID name time selectedSeats
    File.AppendAllText(filePath, details)

// Open the booking file in the default text editor
let openBookingFile time =
    let filePath = sprintf "C:\\Users\\amrya\\Desktop\\ConsoleApp1\\ConsoleApp1\\ConsoleApp1\\ticket details\\%s_bookingDetails.txt" time
    if File.Exists(filePath) then
        Process.Start("notepad.exe", filePath) |> ignore
    else
        MessageBox.Show("No reservation file found for this time.", "File Not Found") |> ignore
