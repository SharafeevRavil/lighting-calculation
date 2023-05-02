using GeometRi;
using Illumination.Entities;
using Illumination.Entities.Basic;
using Illumination.Entities.Hemicube;
using Illumination.Entities.RealObjects;
using Illumination.Util;

namespace Illumination.Services;

public static class FfCalculationService
{
    private class CellWithUsedMark
    {
        public HemicubeCell Cell { get; }
        public bool IsUsed { get; set; }

        public CellWithUsedMark(HemicubeCell cell, bool isUsed)
        {
            Cell = cell;
            IsUsed = isUsed;
        }
    }

    public static FfMatrix CalculateFormFactors(this List<Patch> patches, Hemicube reference)
    {
        var matrix = new FfMatrix();
        foreach (var p1 in patches)
            matrix[p1] = CalculateFormFactors(p1, reference, patches.Where(p2 => p2 != p1));

        return matrix;
    }

    public static Dictionary<Patch, double> CalculateFormFactors(this Patch patch, Hemicube reference,
        IEnumerable<Patch> otherPatches)
    {
        var formFactors = new Dictionary<Patch, double>();

        //sort patches by the distance to the current
        var orderedPatches =
            otherPatches
                .OrderBy(x => x.GetDistance(patch))
                .ToList();
        //build hemicube on the patch
        var hemicube = reference.Copy(patch.Center.ToVector, patch.Normal);

        //form-factor calculation:
        //create list of each hemicube cell with mark if it used or not
        var tempFaces = hemicube.Faces
            .Select(face => (face, cells: face.Cells
                .Select(cell => new CellWithUsedMark(cell, false))
                .ToList()))
            .ToList();
        foreach (var otherPatch in orderedPatches)
        {
            //check if patches are visible to each other BY NORMALS
            if (!patch.IsVisibleFrom(otherPatch))
            {
                formFactors.Add(otherPatch, 0d);
                continue;
            }
            var ff = 0d;
            foreach (var (face, cells) in tempFaces)
            {
                //create patch projection polygon to the whole face
                var patchProj = otherPatch.ConicProjection(patch.Center, face.Polygon.Plane3d);
                if (patchProj == null) continue;
                foreach (var tempCell in cells)
                {
                    if (tempCell.IsUsed) continue;
                    //check if cell intersects projection
                    if (!tempCell.Cell.Polygon.Intersects(patchProj)) continue;

                    //raycast check another objects
                    if (IlluminationConfig.UseRayCastBetweenPatchAndCell)
                    {
                        var cellCenter = tempCell.Cell.Polygon.Center;
                        var ray = new Ray3d(cellCenter, (otherPatch.Center - cellCenter).ToVector);
                        var distanceSqr = cellCenter.DistanceSquared(otherPatch.Center);
                        if (orderedPatches.Where(patch3 => patch3 != patch)
                            .Select(patch3 => ray.IntersectionWith(patch3))
                            .Any(inter => inter != null && inter.Value.distanceSqr < distanceSqr))
                            continue;
                    }

                    ff += tempCell.Cell.DeltaFf;
                    tempCell.IsUsed = true;
                }
            }

            formFactors.Add(otherPatch, ff);
        }

        return formFactors;
    }

    public static bool IsVisibleFrom(this Polygon p1, Polygon p2)
    {
        var diff = (p2.Center - p1.Center).ToVector;
        var dot1 = p1.Normal.Dot(diff);
        var dot2 = p2.Normal.Dot(-diff);
        return dot1 > 0 && dot2 > 0;
    }
}