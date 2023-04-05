namespace Illumination.Entities;

/// <summary>
/// Полукуб полигона, на который проецируются другие полигоны для вычисления их иллюминации от него
/// </summary>
public class Hemicube
{
    public Hemicube(int cellCount)
    {
        //TODO: Создать стороны полукуба, грань которого состоит из cellCount ячеек
    }

    public List<HemicubeFace> Faces { get; set; }
}