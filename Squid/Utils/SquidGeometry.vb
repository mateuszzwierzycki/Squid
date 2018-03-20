Imports Rhino.Geometry

Module SquidGeometry

    Function DivideAlternative(ByVal x As Curve, ByVal angle As Double) As Polyline
        Dim nplt As New Rhino.Geometry.PolylineCurve(x.ToPolyline(0, 0, angle, 0, 0, 0, 0, 0, True))
        Dim npl As New Polyline()
        nplt.TryGetPolyline(npl)
        Return npl
    End Function


    Function DivideCurve(Crv As Curve, TargetL As Double) As List(Of Point3d)
        Dim Pline As New Polyline

        If Crv.IsPolyline Then
            Crv.TryGetPolyline(Pline)
            Return Pline.ToList
        End If

        Dim Pts As New List(Of Point3d)

        Dim Seg As New List(Of Curve)(SplitToSegments(Crv))

        If Seg.Count = 0 Then Seg.Add(Crv)

        Dim ThisLength As Double
        Dim Params As New List(Of Double)
        Dim C As Curve
        Dim Interval01 As New Rhino.Geometry.Interval(0, 1)
        Dim rhinoAcc As Double = Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance
        Dim targetInv As Double = 1 / TargetL
        Dim thisByTarget As Double

        For i As Integer = 0 To Seg.Count - 1 Step 1
            C = Seg(i)
            Params.Clear()
            ThisLength = C.GetLength
            C.Domain = Interval01
            thisByTarget = Math.Round(ThisLength * targetInv)

            Params.Add(0)

            If (thisByTarget < 1 And TargetL > 0) Then

            ElseIf C.IsLinear(rhinoAcc) Then

            ElseIf TargetL = -1 Then
                Params.AddRange(C.DivideByCount(10, False))
            Else
                Params.AddRange(C.DivideByCount(thisByTarget, False))
            End If

            For Each p As Double In Params
                Pts.Add(C.PointAt(p))
            Next

        Next

        Pts.Add(Crv.PointAtEnd)

        Return Pts

    End Function

    Function SplitToSegments(Crv As Curve) As List(Of Curve)

        Crv.Domain = New Rhino.Geometry.Interval(0, 1)
        Dim ParamList As New List(Of Double)
        Dim Param As Double = 0

        Dim DoMore As Boolean = True

        While DoMore
            DoMore = False
            Crv.GetNextDiscontinuity(Continuity.C2_locus_continuous, Param, 1, Param)
            If Rhino.RhinoMath.IsValidDouble(Param) Then
                DoMore = True
                ParamList.Add(Param)
            Else
                DoMore = False
            End If
        End While

        Dim CrvList As New List(Of Curve)(Crv.Split(ParamList))

        Return CrvList

    End Function

    Function PointToRect(pt As Point3d, DrawRect As Rhino.Geometry.Rectangle3d, Pix As Double) As Drawing.Rectangle

        Dim center As New Drawing.Point(OrientPoint(pt, DrawRect, Pix))
        Dim rad As Double = pt.Z * Pix

        Return New Drawing.Rectangle(center.X - rad, center.Y - rad, rad + rad, rad + rad)
    End Function

    Function OrientPoint(Pt As Point3d, DrawRect As Rhino.Geometry.Rectangle3d, Pix As Double) As Drawing.Point
        Dim np As New Drawing.Point
        Dim nv As New Vector3d(Pt - DrawRect.PointAt(3))

        Return New Drawing.Point(nv.X * Pix, -nv.Y * Pix)

    End Function

    Function LineToCircle(L As Line, DrawRect As Rectangle3d, Pix As Double) As Drawing.Rectangle

        Dim ps As New Drawing.Point(OrientPoint(L.From, DrawRect, Pix))

        Dim len As Double = L.Length * Pix

        Return New Drawing.Rectangle(ps.X - len, ps.Y - len, len * 2, len * 2)

    End Function

End Module
