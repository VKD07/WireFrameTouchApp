using System;

[Serializable]
public class Face
{
    public string name;
    public Line line1;
    public Line line2;
    public Line line3;

    public Face()
    {

    }

    public Face(string name, Line line1, Line line2, Line line3)
    {
        this.name = name;
        this.line1 = line1;
        this.line2 = line2;
        this.line3 = line3;
    }

    public Face(Line line1, Line line2, Line line3)
    {
        this.line1 = line1;
        this.line2 = line2;
        this.line3 = line3;
    }

    public static bool CompareFace(Face p_face1, Face p_face2)
    {

        return false;
    }

}