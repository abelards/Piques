﻿module Client

open System.Drawing
open System.Net.Sockets
open System.Threading
open System.Windows.Forms

open Game
open Network

let form = new Form(Text = "Batailles et piques", Width = 700, Height = 400)
let topText = new Label(Text = "Non connecté", Left = 50, Top = 10, Width = 500)

let players = ["Rubix"; "Nicuvëo"] |> List.map (fun s -> new Player(s))
let myId = 0

let updateDisplay () =
    form.Controls.Clear()
    form.Controls.Add(topText)
    let mutable top = 50

    // Action buttons
    form.Controls.Add(new Label(Text = "Actions", Top = top))
    let button = new Button(Text = "Attaquer", Left = 100, Top = top)
    button.MouseClick.Add(fun _ -> System.Windows.Forms.MessageBox.Show("test") |> ignore)
    // button.BackColor <- Color.Red;
    form.Controls.Add(button)

    let button = new Button(Text = "Tire-au-flanc", Left = 200, Top = top)
    button.MouseClick.Add(fun _ -> System.Windows.Forms.MessageBox.Show("test") |> ignore)
    form.Controls.Add(button)
    top <- top + 50

    // Display hand
    let player = players.[myId]
    let hand = player.Hand
    for i in 0 .. hand.Length - 1 do
        form.Controls.Add(new Label(Text = "Ta main", Top = top))
        let button = new Button(Text = hand.[i].ToString(), Left = 100 + i * 100, Top = top)
        button.MouseClick.Add(fun _ -> System.Windows.Forms.MessageBox.Show(hand.[i].ToString()) |> ignore)
        form.Controls.Add(button)
    top <- top + 50

    // Player buttons
    for p in players do
        form.Controls.Add(new Label(Text = p.Name, Top = top))

        let button1 = new Button(Text = "foo", Left = 100, Top = top)
        button1.MouseClick.Add(fun _ -> System.Windows.Forms.MessageBox.Show("test") |> ignore)
        form.Controls.Add(button1)
        let button2 = new Button(Text = "foo", Left = 200, Top = top)
        button2.MouseClick.Add(fun _ -> System.Windows.Forms.MessageBox.Show("test2") |> ignore)
        form.Controls.Add(button2)
        top <- top + 50

let updateText message =
    topText.Text <- message

updateDisplay()
updateText "Toujours pas connecté"

let doNetwork = async {
    let tcp = new TcpClient()
    tcp.Connect("localhost", 3000)
    let text = "LLB"
    do! tcp.GetStream().AsyncWriteString(text)
    let! msg = tcp.GetStream().AsyncReadString
    do! Async.Sleep(5000)
    updateText (msg.ToString())
}

Async.StartImmediate doNetwork
Application.Run(form)