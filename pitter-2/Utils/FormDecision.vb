Public Class FormDecision
    Public Function choice()
        If My.Forms.WebApp.IsHandleCreated = True Then
            Return WebApp
        End If
        If My.Forms.PitterMini.IsHandleCreated = True Then
            Return PitterMini
        End If
    End Function
End Class