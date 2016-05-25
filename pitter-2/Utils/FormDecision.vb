Public Class FormDecision
    Public Function choice()
        If My.Forms.WebApp.IsDisposed = False Then
            Return WebApp
        End If
        If My.Forms.PitterMini.IsDisposed = False Then
            Return PitterMini
        End If
    End Function
End Class