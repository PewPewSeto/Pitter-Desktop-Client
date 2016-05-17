Imports System
Imports System.Security.Cryptography
Imports System.IO
Imports System.Drawing.Imaging

Public Class Encryption
    Private Shared s_aditionalEntropy As Byte() = {4, 6, 5, 4, 5, 3, 4}
    Public Shared Function Protect(ByVal data() As Byte) As Byte()
        Try
            ' Encrypt the data using DataProtectionScope.CurrentUser. The result can be decrypted
            '  only by the same current user.
            Return ProtectedData.Protect(data, s_aditionalEntropy, DataProtectionScope.CurrentUser)
        Catch e As CryptographicException
            Console.WriteLine("Data was not encrypted. An error occurred.")
            Console.WriteLine(e.ToString())
            Return Nothing
        End Try

    End Function
    Public Shared Function Unprotect(ByVal data() As Byte) As Byte()
        Try
            'Decrypt the data using DataProtectionScope.CurrentUser.
            Return ProtectedData.Unprotect(data, s_aditionalEntropy, DataProtectionScope.CurrentUser)
        Catch e As CryptographicException
            Console.WriteLine("Data was not decrypted. An error occurred.")
            Console.WriteLine(e.ToString())
            Return Nothing
        End Try

    End Function
    Public Function DPAPI_encrypt(ByVal data As String)
        Try
            Dim data_bytes As Byte() = System.Text.Encoding.Unicode.GetBytes(data)
            Dim encrypted_data = Protect(data_bytes)

            Dim byte_string_f = ""
            For Each c_byte As Byte In encrypted_data
                byte_string_f += c_byte.ToString + ","
            Next

            byte_string_f = byte_string_f.Substring(0, byte_string_f.Length - 1)

            Return byte_string_f
        Catch ex As Exception
            Return "0"
        End Try
    End Function
    Public Function DPAPI_decrpyt(ByVal byte_string As String)
        Try
            Dim encrypted_string_byte_list As New List(Of Byte)

            For Each c_byte As Byte In byte_string.Split(",")
                encrypted_string_byte_list.Add(c_byte)
            Next

            Dim data_bytes As Byte() = Unprotect(encrypted_string_byte_list.ToArray)

            Return System.Text.Encoding.Unicode.GetString(data_bytes)
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Function base64_encode(ByVal data As String)
        Dim string_bytes As Byte() = System.Text.Encoding.UTF8.GetBytes(data)
        Return System.Convert.ToBase64String(string_bytes)
    End Function
    Public Function ConvertFileToBase64(ByVal fileName As String) As String
        'Converts file contents to Base64.
        Return Convert.ToBase64String(System.IO.File.ReadAllBytes(fileName))
    End Function
    Public Function ConvertImageToBase64(ByVal imagepath As String)
        'Convert image to Base64

        Dim img As Image = CType(Image.FromFile(imagepath, True), Image)
        Using memStream As New MemoryStream
            img.Save(memStream, ImageFormat.Png)
            Dim result As String = Convert.ToBase64String(memStream.ToArray())
            memStream.Close()
            Return result
        End Using
    End Function
End Class
