using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

public class Rating
{
    public static CupcakeRating Analyze(RenderTarget2D canvas, string imagePath)
    {
        // read all pixels from canvas
        Color[] pixels = new Color[canvas.Width * canvas.Height];
        canvas.GetData(pixels);

        float coverage    = GetCoverage(pixels);
        float variety     = GetColorVariety(pixels);
        float distribution = GetDistribution(pixels, canvas.Width, canvas.Height);

        // weight each metric
        float score = (coverage * 0.4f) + (variety * 0.3f) + (distribution * 0.3f);

        // convert 0-1 score to 1-5 stars
        int stars = (int)Math.Round(score * 4) + 1;
        stars = Math.Clamp(stars, 1, 5);

        return new CupcakeRating
        {
            ImagePath = imagePath,
            Stars = stars,
            Coverage = coverage,
            Variety = variety,
            Distribution = distribution
        };
    }

    // what % of pixels have been painted
    private static float GetCoverage(Color[] pixels)
    {
        int painted = 0;
        foreach (var p in pixels)
            if (p.A > 10)   // ignore nearly transparent pixels
                painted++;

        float coverage = (float)painted / pixels.Length;

        // sweet spot is 40-80% coverage — too little or too much penalizes
        if (coverage < 0.1f) return coverage * 2f;
        if (coverage > 0.9f) return 0.7f;
        return Math.Min(coverage * 1.5f, 1f);
    }

    // how many distinct colors were used
    private static float GetColorVariety(Color[] pixels)
    {
        HashSet<Color> uniqueColors = new HashSet<Color>();

        foreach (var p in pixels)
        {
            if (p.A > 10)
            {
                // bucket colors into groups so similar shades count as one
                Color bucketed = new Color(
                    (p.R / 32) * 32,
                    (p.G / 32) * 32,
                    (p.B / 32) * 32
                );
                uniqueColors.Add(bucketed);
            }
        }

        // 1 color = low score, 5+ colors = full score
        return Math.Min(uniqueColors.Count / 5f, 1f);
    }

    // is the paint spread around or all in one corner?
    private static float GetDistribution(Color[] pixels, int width, int height)
    {
        // divide canvas into a 3x3 grid, check if each zone has paint
        int zoneW = width / 3;
        int zoneH = height / 3;
        int zonesWithPaint = 0;

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                bool hasPaint = false;
                for (int y = row * zoneH; y < (row + 1) * zoneH && !hasPaint; y++)
                {
                    for (int x = col * zoneW; x < (col + 1) * zoneW && !hasPaint; x++)
                    {
                        if (pixels[y * width + x].A > 10)
                            hasPaint = true;
                    }
                }
                if (hasPaint) zonesWithPaint++;
            }
        }

        // 9 zones total — more zones covered = better score
        return zonesWithPaint / 9f;
    }
}