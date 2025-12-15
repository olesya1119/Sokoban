using MonoGameLibrary;
using System;

namespace Sokoban.Scenes.UI;

class LevelMenuScene : InterfaceScene
{
    private readonly int _levelsCount = 5;
    private const int maxLevelsInRow = 3;

    public LevelMenuScene() : base()
    {
        int levelDrawCount = 1;
        int countLevelsInColumn = (int)Math.Ceiling(((double)_levelsCount) / maxLevelsInRow);
        float startX = 80;
        float startY = 20;
        float stepX = 60;
        float stepY = 40;

        for (var i = 0; i < countLevelsInColumn && levelDrawCount <= _levelsCount; i++)
        {
            for (var j = 0; j < maxLevelsInRow && levelDrawCount <= _levelsCount; j++)
            {
                var levelNumber = levelDrawCount;

                AddButton(
                    text: levelDrawCount.ToString(),
                    width: 40,
                    height: 10,
                    x: startX + j * stepX,
                    y: startY + i * stepY,
                    action: (s, e) => StartLevel(levelNumber)
                );

                levelDrawCount++;
            }
        }

        AddButton("Back", 100, -10, 110, 130, (s, e) => Core.ChangeScene(new TitleScene()));
    }

    private void StartLevel(int levelNumber)
    {
        Core.ChangeScene(new GameScene(levelNumber));
    }
}
