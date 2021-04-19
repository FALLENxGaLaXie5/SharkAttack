using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Dasbor.Scripts.Interfaces
{
    public interface IBrain
    {
        Vector2 GetDirection();

        void SetDirection(Vector2 direction);
    }
}
