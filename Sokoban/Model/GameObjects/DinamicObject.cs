using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Core.Entities
{
    /// <summary>
    /// Базовый класс динамического объекта (объектов способных к измнению позиции)
    /// </summary>
    public abstract class DinamicObject
    {
        /// <summary>
        /// Можно ли толкать этот объект
        /// </summary>
        public abstract bool isPusheble { get; }

        /// <summary>
        /// Можно ли перемещаться по этому объекту
        /// </summary>
        public abstract bool IsWalkalbe { get; }


    }
}
