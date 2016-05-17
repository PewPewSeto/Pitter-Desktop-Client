Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Collections

Namespace norvanco.http
    ''' <summary>
    ''' Allow the transfer of data files using the W3C's specification
    ''' for HTTP multipart form data.  Microsoft's version has a bug
    ''' where it does not format the ending boundary correctly.
    ''' Written by: gregoryp@norvanco.com
    ''' </summary>
    Public Class MultipartForm
        ''' <summary>
        ''' Holds any form fields and values that you
        ''' wish to transfer with your data.
        ''' </summary>
        Private coFormFields As Hashtable
        ''' <summary>
        ''' Used mainly to avoid passing parameters to other routines.
        ''' Could have been local to sendFile().
        ''' </summary>
        Protected coRequest As HttpWebRequest
        ''' <summary>
        ''' Used if we are testing and want to output the raw
        ''' request, minus http headers, out to a file.
        ''' </summary>
        Private coFileStream As System.IO.Stream
        ''' <summary>
        ''' Difined to build the form field data that is being
        ''' passed along with the request.
        ''' </summary>
        Shared CONTENT_DISP As String = "Content-Disposition: form-data; name="
        ''' <summary>
        ''' Allows you to specify the specific version of HTTP to use for uploads.
        ''' The dot NET stuff currently does not allow you to remove the continue-100 header
        ''' from 1.1 and 1.0 currently has a bug in it where it adds the continue-100.  MS
        ''' has sent a patch to remove the continue-100 in HTTP 1.0.
        ''' </summary>
        Public Property TransferHttpVersion() As Version
            Get
                Return coHttpVersion
            End Get
            Set(value As Version)
                coHttpVersion = value
            End Set
        End Property
        Private coHttpVersion As Version

        ''' <summary>
        ''' Used to change the content type of the file being sent.
        ''' Currently defaults to: text/xml. Other options are
        ''' text/plain or binary
        ''' </summary>
        Public Property FileContentType() As String
            Get
                Return coFileContentType
            End Get
            Set(value As String)
                coFileContentType = value
            End Set
        End Property
        Private coFileContentType As String

        ''' <summary>
        ''' Initialize our class for use to send data files.
        ''' </summary>
        ''' <param name="url">The web address of the recipient of the data transfer.</param>
        Public Sub New(url__1 As String)
            URL = url__1
            coFormFields = New Hashtable()
            ResponseText = New StringBuilder()
            BufferSize = 1024 * 10
            BeginBoundary = "ou812--------------8c405ee4e38917c"
            TransferHttpVersion = HttpVersion.Version11
            FileContentType = "text/xml"
        End Sub
        '---------- BEGIN PROPERTIES SECTION ----------
        Private _BeginBoundary As String
        ''' <summary>
        ''' The string that defines the begining boundary of
        ''' our multipart transfer as defined in the w3c specs.
        ''' This method also sets the Content and Ending
        ''' boundaries as defined by the w3c specs.
        ''' </summary>
        Public Property BeginBoundary() As String
            Get
                Return _BeginBoundary
            End Get
            Set(value As String)
                _BeginBoundary = value
                ContentBoundary = Convert.ToString("--") & BeginBoundary
                EndingBoundary = ContentBoundary & Convert.ToString("--")
            End Set
        End Property
        ''' <summary>
        ''' The string that defines the content boundary of
        ''' our multipart transfer as defined in the w3c specs.
        ''' </summary>
        Protected Property ContentBoundary() As String
            Get
                Return _ContentBoundary
            End Get
            Set(value As String)
                _ContentBoundary = value
            End Set
        End Property
        Private _ContentBoundary As String
        ''' <summary>
        ''' The string that defines the ending boundary of
        ''' our multipart transfer as defined in the w3c specs.
        ''' </summary>
        Protected Property EndingBoundary() As String
            Get
                Return _EndingBoundary
            End Get
            Set(value As String)
                _EndingBoundary = value
            End Set
        End Property
        Private _EndingBoundary As String
        ''' <summary>
        ''' The data returned to us after the transfer is completed.
        ''' </summary>
        Public Property ResponseText() As StringBuilder
            Get
                Return _ResponseText
            End Get
            Set(value As StringBuilder)
                _ResponseText = value
            End Set
        End Property
        Private _ResponseText As StringBuilder
        ''' <summary>
        ''' The web address of the recipient of the transfer.
        ''' </summary>
        Public Property URL() As String
            Get
                Return _URL
            End Get
            Set(value As String)
                _URL = value
            End Set
        End Property
        Private _URL As String
        ''' <summary>
        ''' Allows us to determine the size of the buffer used
        ''' to send a piece of the file at a time out the IO
        ''' stream.  Defaults to 1024 * 10.
        ''' </summary>
        Public Property BufferSize() As Integer
            Get
                Return _BufferSize
            End Get
            Set(value As Integer)
                _BufferSize = value
            End Set
        End Property
        Private _BufferSize As Integer
        '---------- END PROPERTIES SECTION ----------
        ''' <summary>
        ''' Used to signal we want the output to go to a
        ''' text file verses being transfered to a URL.
        ''' </summary>
        ''' <param name="path"></param>
        Public Sub setFilename(path As String)
            coFileStream = New System.IO.FileStream(path, FileMode.Create, FileAccess.Write)
        End Sub
        ''' <summary>
        ''' Allows you to add some additional field data to be
        ''' sent along with the transfer.  This is usually used
        ''' for things like userid and password to validate the
        ''' transfer.
        ''' </summary>
        ''' <param name="key">The form field name</param>
        ''' <param name="str">The form field value</param>
        Public Sub setField(key As String, str As String)
            coFormFields(key) = str
        End Sub
        ''' <summary>
        ''' Determines if we have a file stream set, and returns either
        ''' the HttpWebRequest stream of the file.
        ''' </summary>
        ''' <returns></returns>
        Public Overridable Function getStream() As System.IO.Stream
            Dim io As System.IO.Stream
            If coFileStream Is Nothing Then
                io = coRequest.GetRequestStream()
            Else
                io = coFileStream
            End If
            Return io
        End Function
        ''' <summary>
        ''' Here we actually make the request to the web server and
        ''' retrieve it's response into a text buffer.
        ''' </summary>
        Public Overridable Sub getResponse()
            If coFileStream Is Nothing Then
                Dim io As System.IO.Stream
                Dim oResponse As WebResponse
                Try
                    oResponse = coRequest.GetResponse()
                Catch web As WebException
                    oResponse = web.Response
                End Try
                If oResponse IsNot Nothing Then
                    io = oResponse.GetResponseStream()
                    Dim sr As New StreamReader(io)
                    Dim str As String
                    ResponseText.Length = 0
                    While (InlineAssignHelper(str, sr.ReadLine())) IsNot Nothing
                        ResponseText.Append(str)
                    End While
                    oResponse.Close()
                Else
                    Throw New Exception("MultipartForm: Error retrieving server response")
                End If
            End If
        End Sub
        ''' <summary>
        ''' Transmits a file to the web server stated in the
        ''' URL property.  You may call this several times and it
        ''' will use the values previously set for fields and URL.
        ''' </summary>
        ''' <param name="aFilename">The full path of file being transfered.</param>
        Public Sub sendFile(aFilename As String)
            ' The live of this object is only good during
            '  this function.  Used mainly to avoid passing
            '  around parameters to other functions.
            coRequest = DirectCast(WebRequest.Create(URL), HttpWebRequest)
            ' Set use HTTP 1.0 or 1.1.
            coRequest.ProtocolVersion = TransferHttpVersion
            coRequest.Method = "POST"
            coRequest.ContentType = Convert.ToString("multipart/form-data; boundary=") & BeginBoundary
            coRequest.Headers.Add("Cache-Control", "no-cache")
            coRequest.KeepAlive = True
            Dim strFields As String = getFormfields()
            Dim strFileHdr As String = getFileheader(aFilename)
            Dim strFileTlr As String = getFiletrailer()
            Dim info As New FileInfo(aFilename)
            coRequest.ContentLength = strFields.Length + strFileHdr.Length + strFileTlr.Length + info.Length
            Dim io As System.IO.Stream
            io = getStream()
            writeString(io, strFields)
            writeString(io, strFileHdr)
            Me.writeFile(io, aFilename)
            writeString(io, strFileTlr)
            getResponse()
            io.Close()
            ' End the life time of this request object.
            coRequest = Nothing
        End Sub
        ''' <summary>
        ''' Mainly used to turn the string into a byte buffer and then
        ''' write it to our IO stream.
        ''' </summary>
        ''' <param name="io">The io stream for output.</param>
        ''' <param name="str">The data to write.</param>
        Public Sub writeString(io As System.IO.Stream, str As String)
            Dim PostData As Byte() = System.Text.Encoding.ASCII.GetBytes(str)
            io.Write(PostData, 0, PostData.Length)
        End Sub
        ''' <summary>
        ''' Builds the proper format of the multipart data that
        ''' contains the form fields and their respective values.
        ''' </summary>
        ''' <returns>The data to send in the multipart upload.</returns>
        Public Function getFormfields() As String
            Dim str As String = ""
            Dim myEnumerator As IDictionaryEnumerator = coFormFields.GetEnumerator()
            While myEnumerator.MoveNext()
                str += (Convert.ToString(ContentBoundary & Convert.ToString(vbCr & vbLf)) & CONTENT_DISP) + """"c + myEnumerator.Key + """" & vbCr & vbLf & vbCr & vbLf + myEnumerator.Value + vbCr & vbLf
            End While
            Return str
        End Function
        ''' <summary>
        ''' Returns the proper content information for the
        ''' file we are sending.
        ''' </summary>
        ''' <remarks>
        ''' Hits Patel reported a bug when used with ActiveFile.
        ''' Added semicolon after sendfile to resolve that issue.
        ''' Tested for compatibility with IIS 5.0 and Java.
        ''' </remarks>
        ''' <param name="aFilename"></param>
        ''' <returns></returns>
        Public Function getFileheader(aFilename As String) As String
            Return (Convert.ToString((Convert.ToString(ContentBoundary & Convert.ToString(vbCr & vbLf)) & CONTENT_DISP) + """sendfile""; filename=""" + Path.GetFileName(aFilename) + """" & vbCr & vbLf + "Content-type: ") & FileContentType) + vbCr & vbLf & vbCr & vbLf
        End Function
        ''' <summary>
        ''' Creates the proper ending boundary for the multipart upload.
        ''' </summary>
        ''' <returns>The ending boundary.</returns>
        Public Function getFiletrailer() As String
            Return Convert.ToString(vbCr & vbLf) & EndingBoundary
        End Function
        ''' <summary>
        ''' Reads in the file a chunck at a time then sends it to the
        ''' output stream.
        ''' </summary>
        ''' <param name="io">The io stream to write the file to.</param>
        ''' <param name="aFilename">The name of the file to transfer.</param>
        Public Sub writeFile(io As System.IO.Stream, aFilename As String)
            Dim readIn As New FileStream(aFilename, FileMode.Open, FileAccess.Read)
            readIn.Seek(0, SeekOrigin.Begin)
            ' move to the start of the file
            Dim fileData As Byte() = New Byte(BufferSize - 1) {}
            Dim bytes As Integer
            While (InlineAssignHelper(bytes, readIn.Read(fileData, 0, BufferSize))) > 0
                ' read the file data and send a chunk at a time
                io.Write(fileData, 0, bytes)
            End While
            readIn.Close()
        End Sub
        Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
            target = value
            Return value
        End Function
    End Class
End Namespace