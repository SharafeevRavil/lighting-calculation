namespace Illumination.Entities;

/// <summary>
/// Клетка полигона. По числу закрытых проекцией полигона клеток определяется освящённость спроецированного полигона от текущего.
/// </summary>
public class HemicubeCell
{
    public int Depth = int.MaxValue;

    public bool IsCovered(Projection projection, int depth)
    {
        if (depth > Depth)
            return false;
        //TODO: Касается ли проекция ячейки? Если true, то depth = Depth
        throw new NotImplementedException();
    }
}