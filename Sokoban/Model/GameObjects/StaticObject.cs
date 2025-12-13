using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban.Model.GameObjects;

/// <summary>
/// Базовый класс статического объекта (объектов не способных к измнению позиции)
/// </summary>
public abstract class StaticObject
{
    /// <summary>
    /// Можно ли перемещаться по этому объекту
    /// </summary>
    public abstract bool IsWalkalbe { get; }
}

/// <summary>
/// Стена
/// </summary>
public class Wall : StaticObject
{
    public override bool IsWalkalbe => false;
}

/// <summary>
/// Область, куда нужно переместить ящик
/// </summary>
public class BoxArea : StaticObject
{
    public override bool IsWalkalbe => true;

}

/// <summary>
/// Пустое пространство
/// </summary>
public class EmptyArea : StaticObject
{
    public override bool IsWalkalbe => true;
}
