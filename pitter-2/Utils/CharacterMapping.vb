Public Class CharacterMapping
    Function keytoASCIIDESC(ByVal key As String)
        Select Case key
            Case "0"
                Return 48
            Case "1"
                Return 49
            Case "2"
                Return 50
            Case "3"
                Return 51
            Case "4"
                Return 52
            Case "5"
                Return 53
            Case "6"
                Return 54
            Case "7"
                Return 55
            Case "8"
                Return 56
            Case "9"
                Return 57
            Case "F1"
                Return 112
            Case "F2"
                Return 113
            Case "F3"
                Return 114
            Case "F4"
                Return 115
            Case "F5"
                Return 116
            Case "F6"
                Return 117
            Case "F7"
                Return 118
            Case "F8"
                Return 119
            Case "F9"
                Return 120
            Case "F10"
                Return 121
            Case "F11"
                Return 122
            Case "F12"
                Return 123
            Case "Numberpad 0"
                Return 48
            Case "Numberpad 1"
                Return 49
            Case "Numberpad 2"
                Return 50
            Case "Numberpad 3"
                Return 51
            Case "Numberpad 4"
                Return 52
            Case "Numberpad 5"
                Return 53
            Case "Numberpad 6"
                Return 54
            Case "Numberpad 7"
                Return 55
            Case "Numberpad 8"
                Return 56
            Case "Numberpad 9"
                Return 57
            Case "Numberpad Decimal (.)"
                Return 46

        End Select
    End Function
    Function ASCIIDESCtokey(ByVal key As Integer)
        Select Case key
            Case 48
                Return "0"
            Case 49
                Return "1"
            Case 50
                Return "2"
            Case 51
                Return "3"
            Case 52
                Return "4"
            Case 53
                Return "5"
            Case 54
                Return "6"
            Case 55
                Return "7"
            Case 56
                Return "8"
            Case 57
                Return "9"
            Case 112
                Return "F1"
            Case 113
                Return "F2"
            Case 114
                Return "F3"
            Case 115
                Return "F4"
            Case 116
                Return "F5"
            Case 117
                Return "F6"
            Case 118
                Return "F7"
            Case 119
                Return "F8"
            Case 120
                Return "F9"
            Case 121
                Return "F10"
            Case 122
                Return "F11"
            Case 123
                Return "F12"
            Case 48
                Return "Numberpad 0"
            Case 49
                Return "Numberpad 1"
            Case 50
                Return "Numberpad 2"
            Case 51
                Return "Numberpad 3"
            Case 52
                Return "Numberpad 4"
            Case 53
                Return "Numberpad 5"
            Case 54
                Return "Numberpad 6"
            Case 55
                Return "Numberpad 7"
            Case 56
                Return "Numberpad 8"
            Case 57
                Return "Numberpad 9"
            Case 46
                Return "Numberpad Decimal (.)"
        End Select

    End Function

    Public Function formkeys(ByVal key As String)
        Select Case key
            Case "0"
                Return Keys.D0
            Case "1"
                Return Keys.D1
            Case "2"
                Return Keys.D2
            Case "3"
                Return Keys.D3
            Case "4"
                Return Keys.D4
            Case "5"
                Return Keys.D5
            Case "6"
                Return Keys.D6
            Case "7"
                Return Keys.D7
            Case "8"
                Return Keys.D8
            Case "9"
                Return Keys.D9
            Case "F1"
                Return Keys.F1
            Case "F2"
                Return Keys.F2
            Case "F3"
                Return Keys.F3
            Case "F4"
                Return Keys.F4
            Case "F5"
                Return Keys.F5
            Case "F6"
                Return Keys.F6
            Case "F7"
                Return Keys.F7
            Case "F8"
                Return Keys.F8
            Case "F9"
                Return Keys.F9
            Case "F10"
                Return Keys.F10
            Case "F11"
                Return Keys.F11
            Case "F12"
                Return Keys.F12
            Case "Numberpad 0"
                Return Keys.NumPad0
            Case "Numberpad 1"
                Return Keys.NumPad1
            Case "Numberpad 2"
                Return Keys.NumPad2
            Case "Numberpad 3"
                Return Keys.NumPad3
            Case "Numberpad 4"
                Return Keys.NumPad4
            Case "Numberpad 5"
                Return Keys.NumPad5
            Case "Numberpad 6"
                Return Keys.NumPad6
            Case "Numberpad 7"
                Return Keys.NumPad7
            Case "Numberpad 8"
                Return Keys.NumPad8
            Case "Numberpad 9"
                Return Keys.NumPad9
            Case "Numberpad Decimal (.)"
                Return Keys.Delete

        End Select
    End Function
End Class