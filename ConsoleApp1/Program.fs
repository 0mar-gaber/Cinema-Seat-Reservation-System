open System
open System.Drawing
open System.Windows.Forms
open System.IO
open System.Diagnostics


let generateTicketID () =
    "TICKET-" + (Guid.NewGuid().ToString())

let readBookingFromFile time =
    let filePath = sprintf "D:\\f#\\ConsoleApp1\\ConsoleApp1\\ticket details\\%s_bookingDetails.txt" time
    if File.Exists(filePath) then
        File.ReadAllLines(filePath)
    else
        [||]


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
    
      let seats = Array2D.init 4 9 (fun _ _ -> new PictureBox())


    let tableLayoutPanel1 = new TableLayoutPanel()
    tableLayoutPanel1.Location <- Point(68, 248)
    tableLayoutPanel1.Size <- Size(951, 260)
    tableLayoutPanel1.ColumnCount <- 9
    tableLayoutPanel1.RowCount <- 4
    tableLayoutPanel1.BackColor <- Color.Transparent
    for i in 0 .. 8 do
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.1f))
    for i in 0 .. 3 do
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25.0f))
    form.Controls.Add(tableLayoutPanel1)

    let comboBoxTime = new ComboBox()
    comboBoxTime.Font <- new Font("Segoe UI", 19.0f)
    comboBoxTime.Location <- Point(1044, 323)
    comboBoxTime.Size <- Size(278, 27)
    comboBoxTime.Items.AddRange([| "12 PM"; "03 PM"; "06 PM"; "09 PM" |])
    comboBoxTime.SelectedIndex <- 0
    comboBoxTime.DropDownStyle <- ComboBoxStyle.DropDownList
    form.Controls.Add(comboBoxTime)

    let updateSeats (time: string) =
        let bookingDetails = readBookingFromFile time
        tableLayoutPanel1.Controls.Clear()  
        for row in 0 .. 3 do
            for col in 0 .. 8 do
                let seat = new PictureBox()
                seat.Image <- Image.FromFile("D:\\f#\\ConsoleApp1\\ConsoleApp1\\image\\Group 916.png")
                seat.Size <- Size(94, 49)
                seat.SizeMode <- PictureBoxSizeMode.Zoom
                seat.Image.Tag <- "available"
                let seatName = sprintf "Row: %d, Column: %d" (row + 1) (col + 1)
                if bookingDetails |> Array.exists (fun line -> line.Contains(seatName)) then
                    seat.Image <- Image.FromFile("D:\\f#\\ConsoleApp1\\ConsoleApp1\\image\\Group 940.png")
                    seat.Image.Tag <- "reserved"
                seat.Click.Add(fun _ ->
                    if seat.Image.Tag.ToString() = "available" then
                        seat.Image <- Image.FromFile("D:\\f#\\ConsoleApp1\\ConsoleApp1\\image\\Group 906.png")
                        seat.Image.Tag <- "selected"
                    else if seat.Image.Tag.ToString() = "selected" then
                        seat.Image <- Image.FromFile("D:\\f#\\ConsoleApp1\\ConsoleApp1\\image\\Group 916.png")
                        seat.Image.Tag <- "available"
                    else
                        MessageBox.Show("Sorry, this seat is already reserved.", "Seat Information") |> ignore
                )
                tableLayoutPanel1.Controls.Add(seat, col, row)  
                seats.[row, col] <- seat

    updateSeats (comboBoxTime.SelectedItem.ToString())  

    comboBoxTime.SelectedIndexChanged.Add(fun _ ->
        updateSeats (comboBoxTime.SelectedItem.ToString())  
    )

    let labelName = new Label()
    labelName.Text <- "Name:"
    labelName.AutoSize <- true
    labelName.BackColor <- Color.Transparent
    labelName.Font <- new Font("Segoe UI Semibold", 20.0f)
    labelName.ForeColor <- Color.LightSeaGreen
    labelName.Location <- Point(1044, 136)
    form.Controls.Add(labelName)
    labelName.BringToFront()

    let textBoxName = new TextBox()
    textBoxName.BackColor <- Color.SeaShell
    textBoxName.Font <- new Font("Segoe UI", 20.0f)
    textBoxName.Location <- Point(1044, 182)
    textBoxName.Size <- Size(278, 52)
    form.Controls.Add(textBoxName)

     let buttonBook = new Button()
    buttonBook.Text <- "Book"
    buttonBook.Font <- new Font("Segoe UI", 13.8f, FontStyle.Bold)
    buttonBook.BackColor <- Color.Black
    buttonBook.ForeColor <- Color.LightSeaGreen
    buttonBook.Location <- Point(1131, 404)
    buttonBook.Size <- Size(113, 47)
    buttonBook.Click.Add(fun _ ->
        if String.IsNullOrWhiteSpace(textBoxName.Text) then
            textBoxName.BackColor <- Color.LightCoral
        else
            textBoxName.BackColor <- Color.SeaShell
            let name = textBoxName.Text
            let selectedSeats =
                [ for row in 0 .. 3 do
                  for col in 0 .. 8 do
                      let seat = seats.[row, col]
                      if seat.Image.Tag = "selected" then
                          yield sprintf "Row: %d, Column: %d" (row+1) (col+1) ]
            let time = comboBoxTime.SelectedItem.ToString()
            if selectedSeats |> List.isEmpty || String.IsNullOrWhiteSpace(name) then
                MessageBox.Show("Failed: No seats reserved.", "Booking Confirmation Failed") |> ignore
            else
                let ticketID = generateTicketID()
                saveBookingToFile name time (String.Join("\n", selectedSeats)) ticketID
                let message = sprintf "Ticket ID: %s\nName: %s\nTime: %s\nReserved Seats:\n%s" ticketID name time (String.Join("\n", selectedSeats))
                MessageBox.Show(message, "Booking Confirmation") |> ignore
                updateSeats time  
    )
    form.Controls.Add(buttonBook)



    
    form
