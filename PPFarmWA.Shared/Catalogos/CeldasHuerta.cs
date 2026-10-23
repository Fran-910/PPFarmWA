using System;
using System.Collections.Generic;
using System.Text;

namespace PPFarmWA.Shared.Catalogos
{
    public static class CeldasHuerta
    {
        public static Dictionary<int, int> values = new()
        {
            [10] = 3,
            [20] = 6,
            [30] = 9,
            [40] = 12
        };

        // Tope de columnas/altura del grid. El tamano de celda se calcula asumiendo
        // como mucho 3 filas (ver --celda-max en HuertaCeldasComponent.razor.css).
        private const int MaxColumnas = 5;
        private const int MaxFilas = 3;

        // Devuelve la mayor cantidad de columnas entre 1 y MaxColumnas que divida
        // exactamente la cantidad de celdas y no pase de MaxFilas filas. Asi la
        // cuadricula queda siempre completa, sin tracks vacios en la ultima fila.
        public static int ColumnasPara(int celdas)
        {
            if (celdas <= 0) return 1;

            for (int columnas = Math.Min(MaxColumnas, celdas); columnas >= 1; columnas--)
            {
                if (celdas % columnas == 0 && celdas / columnas <= MaxFilas)
                {
                    return columnas;
                }
            }

            return Math.Min(MaxColumnas, celdas);
        }
    }
}
