Public Class StringTool
    Public Function parse_boolean(ByVal val As String)
        Select Case val
            Case "false", "False", "FALSE", "0"
                Return False
            Case "true", "True", "TRUE", "1"
                Return True
        End Select
        Return False
    End Function
    Public Function StringToUTF8(ByVal str As String) As String
        Dim utf8Encoding As New System.Text.UTF8Encoding(True)
        Dim encodedString() As Byte
        encodedString = utf8Encoding.GetBytes(str)
        Return utf8Encoding.GetString(encodedString)
    End Function

End Class