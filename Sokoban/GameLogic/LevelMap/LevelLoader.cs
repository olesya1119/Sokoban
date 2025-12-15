using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml;

namespace Sokoban.GameLogic;

public static class LevelLoader
{
    public static Level FromXml(ContentManager content, string filename)
    {
        string filePath = Path.Combine(content.RootDirectory, filename);

        using Stream stream = TitleContainer.OpenStream(filePath);
        using XmlReader reader = XmlReader.Create(stream);
        XDocument doc = XDocument.Load(reader);

        XElement root = doc.Root ?? throw new InvalidOperationException("XML: Не соответстует требуемому формату");

        var rowElements = root.Element("Rows")?.Elements("Row");
        if (rowElements == null) throw new InvalidOperationException("XML: Не соответстует требуемому формату");

        List<string> rows = new();
        foreach (var r in rowElements)
            rows.Add(r.Value.Replace("\r", ""));

        if (rows.Count == 0)
            throw new InvalidOperationException("XML: Не соответстует требуемому формату");

        int srcH = rows.Count;
        int srcW = 0;
        foreach (var s in rows)
            srcW = Math.Max(srcW, s.Length);

        if (srcW > Map.MaxWidth || srcH > Map.MaxHeight)
            throw new InvalidOperationException($"Уровень слишком большой big: {srcW}x{srcH}. Максимальный размер: {Map.MaxWidth}x{Map.MaxHeight}.");

        int offsetX = (Map.MaxWidth - srcW) / 2;
        int offsetY = (Map.MaxHeight - srcH) / 2;

        Map map = new Map(Map.MaxWidth, Map.MaxHeight);

        foreach (var p in map.EnumerateAll())
            map.SetCell(p, CellType.Void);

        Point? player = null;
        List<Point> boxes = new();
        HashSet<Point> goals = new();

        int goalsCount = 0;
        int boxesCount = 0;


        for (int y = 0; y < srcH; y++)
        {
            string line = rows[y];

            for (int x = 0; x < srcW; x++)
            {
                char c = x < line.Length ? line[x] : ' ';

                int mx = x + offsetX;
                int my = y + offsetY;

                if (mx < 0 || my < 0 || mx >= Map.MaxWidth || my >= Map.MaxHeight)
                    continue;

                switch (c)
                {
                    case ' ':
                        map.SetCell(mx, my, CellType.Void);
                        break;

                    case '.':
                        map.SetCell(mx, my, CellType.Floor);
                        break;

                    case '#':
                        map.SetCell(mx, my, CellType.Wall);
                        break;

                    case 'G':
                        map.SetCell(mx, my, CellType.Goal);
                        goals.Add(new Point(mx, my));
                        goalsCount++;
                        break;

                    case 'B':
                        map.SetCell(mx, my, CellType.Floor);
                        boxes.Add(new Point(mx, my));
                        boxesCount++;
                        break;

                    case 'P':
                        map.SetCell(mx, my, CellType.Floor);
                        player = new Point(mx, my);
                        break;

                    default:
                        throw new InvalidOperationException($"Неизвестный символ '{c}' at ({x},{y}).");
                }
            }
        }

        if (!player.HasValue)
            throw new InvalidOperationException("Карта должна иметь игрока.");

        if (goalsCount != boxesCount)
            throw new InvalidOperationException($"Количетво мест для ящика (G)={goalsCount} должно быть равно количеству коробок (B)={boxesCount}.");

        return new Level(map, player.Value, boxes, goals);
    }
}
