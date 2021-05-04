
' Anthony Griego
' Final Project
' Purpose: Shopping app that allows user to manipulate shopping cart
' The app allows import Of DVD list text file And export Of customer orders


Option Strict On
Option Explicit On
Option Infer Off

Public Class frmMain

    Const dblTaxRate As Double = 0.04
    Const dblShippingRate As Double = 1
    Const dblMaxShipping As Double = 5

    Dim strDVD(9) As String
    Dim dblDVDPrice(9) As Double
    Dim dblSubTotal As Double
    Dim dblTax As Double
    Dim dblShipping As Double
    Dim dblTotal As Double

    ' This Indepedent sub generates an alert if a required file is not found
    Private Sub NoFileAlert(ByVal strFileName As String)
        MessageBox.Show("Cannot find the file " & strFileName, "No File Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    End Sub


    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles Me.Load

        ' Declare Stream reader variable
        Dim inFile As IO.StreamReader

        ' File reader
        ' Checks if file exists, then opens the text and reads each line. Assigns each line to an array variable
        If IO.File.Exists("availableDVDs.txt") Then
            inFile = IO.File.OpenText("availableDVDs.txt")
            ' Once nothing is found, inFile. Peek will = -1 and stop the loop
            Do Until inFile.Peek = -1
                Dim intIndex As Integer
                strDVD(intIndex) = inFile.ReadLine
                ' Only adds to item list if the string is not empty
                If strDVD(intIndex) <> Nothing Then
                    lstDVD.Items.Add(strDVD(intIndex).PadLeft(55))
                End If
                intIndex += 1
            Loop
            lstDVD.SelectedIndex = 0
            inFile.Close()
            ' Alert if file not found
        Else NoFileAlert("availableDVDs.txt")
        End If

        ' Populates the DVD prices array
        For intNum As Integer = 0 To 9
            ' Trims the DVD string so tha tryparse works properly
            Dim strDVDTrim(9) As String
            strDVDTrim(intNum) = strDVD(intNum).Substring(strDVD(intNum).IndexOf(",") + 2)
            Double.TryParse(strDVDTrim(intNum), dblDVDPrice(intNum))
        Next

        ' Creates customer output file
        Dim outFile As IO.StreamWriter
        outFile = IO.File.CreateText("customerSales.txt")
        outFile.WriteLine("Customer #     Subtotal     Sales Tax     Shipping Charge     Total Cost")
        outFile.Close()

    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        ' Adds the selected item to the shopping cart list and updates subtotals
        lstCart.Items.Add(lstDVD.SelectedItem.ToString.PadLeft(55))

        dblSubTotal += dblDVDPrice(lstDVD.SelectedIndex)
        dblTax = dblSubTotal * dblTaxRate
        dblShipping = Math.Min(lstCart.Items.Count * dblShippingRate, dblMaxShipping)
        dblTotal = dblSubTotal + dblTax + dblShipping

        lblSubtotal.Text = dblSubTotal.ToString("N2")
        lblTax.Text = dblTax.ToString("N2")
        lblShipping.Text = dblShipping.ToString("N2")
        lblTotal.Text = dblTotal.ToString("C2")

    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        ' Removes the selected item from the shopping cart list and updates subtotals
        If lstCart.Items.Contains(lstDVD.SelectedItem.ToString) = True Then
            lstCart.Items.Remove(lstDVD.SelectedItem.ToString)

            dblSubTotal -= dblDVDPrice(lstDVD.SelectedIndex)
            dblTax = dblSubTotal * dblTaxRate
            dblShipping = Math.Min(lstCart.Items.Count * dblShippingRate, dblMaxShipping)
            dblTotal = dblSubTotal + dblTax + dblShipping

            lblSubtotal.Text = dblSubTotal.ToString("N2")
            lblTax.Text = dblTax.ToString("N2")
            lblShipping.Text = dblShipping.ToString("N2")
            lblTotal.Text = dblTotal.ToString("C2")

        End If
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click

        If txtCustomerNumber.TextLength = 4 Then

            ' Text file output using contents of shopping cart
            Dim outFile As IO.StreamWriter
            outFile = IO.File.AppendText("customerSales.txt")
            outFile.WriteLine(txtCustomerNumber.Text.ToString & "           " & lblSubtotal.Text.ToString & "        " & lblTax.Text.ToString & "          " & lblShipping.Text.ToString & "                " & lblTotal.Text.ToString)
            outFile.Close()

            ' Clear shopping cart list, customer #, and reset variables
            lstCart.Items.Clear()

            txtCustomerNumber.Text = String.Empty
            lblSubtotal.Text = String.Empty
            lblTax.Text = String.Empty
            lblShipping.Text = String.Empty
            lblTotal.Text = String.Empty

            dblSubTotal = 0
            dblTax = 0
            dblShipping = 0
            dblTotal = 0

        Else
            MessageBox.Show("Please enter a 4 digit customer number before clearing the shopping cart", "Customer number not valid", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        ' Exits the app
        Me.Close()
    End Sub

    Private Sub txtCustomerNumber_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCustomerNumber.KeyPress
        ' Customer textbox only accepts only non-zero numbers and backspace
        If (e.KeyChar < "1" OrElse e.KeyChar > "9") AndAlso e.KeyChar <> ControlChars.Back Then
            e.Handled = True
        End If
    End Sub



End Class
