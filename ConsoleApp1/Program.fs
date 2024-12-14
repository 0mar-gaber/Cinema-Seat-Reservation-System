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

let createForm () =
    let form = new Form()
    form.Text <- "Cinema Seat Reservation"
    form.ClientSize <- Size(1368, 725)
    form.BackColor <- SystemColors.ActiveCaptionText

    let pictureBox1 = new PictureBox()
    pictureBox1.Image <- Image.FromFile("D:\\f#\\ConsoleApp1\\ConsoleApp1\\image\\Vector 1 (1).png")
    pictureBox1.Location <- Point(81, 17)
    pictureBox1.Size <- Size(906, 142)
    pictureBox1.SizeMode <- PictureBoxSizeMode.StretchImage
    form.Controls.Add(pictureBox1)

    let labelScreen = new Label()
    labelScreen.Text <- "Screen"
    labelScreen.AutoSize <- true
    labelScreen.BackColor <- Color.Transparent
    labelScreen.Font <- new Font("Segoe UI Semibold", 20.0f)
    labelScreen.ForeColor <- Color.LightSeaGreen
    labelScreen.Location <- Point(468, 66)
    form.Controls.Add(labelScreen)
    labelScreen.BringToFront()
    
    
    
    form
