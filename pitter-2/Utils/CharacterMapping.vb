Imports System.Windows.Input
Public Class CharacterMapping
    Public Function keyValidator(ByVal passed_key As Integer, ByVal csr_ As Boolean)
        If csr_ Then
            'Control and Shift is required.
            Select Case passed_key
                Case 97 'keypad 1
                    Return 35 'end
                Case 98 'keypad 2
                    Return 40 'down arrow
                Case 98 'keypad 2
                    Return 40 'down arrow
                Case 99 'keypad 3
                    Return 34 'page down
                Case 100 'keypad 4
                    Return 37 'left arrow
                Case 102 'keypad 6
                    Return 39 'right arrow
                Case 103 'keypad 7
                    Return 36 'home
                Case 104 'keypad 8
                    Return 38 'up arrow
                Case 105 'keypad 9
                    Return 33 'page up
                Case 96 'keypad 0
                    Return 45 'insert
                Case 110 'keypad decimal
                    Return 46 'delete
                Case Else
                    Return passed_key
            End Select
        Else
            'Contrl and Shift isn't required.
            Return passed_key
        End If
    End Function

End Class