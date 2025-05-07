using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsynCollabPDF.Models
{
    interface IPagina
    {
        int indexPagina { get; }
        // Enviar neste tipo de imagem aumenta acoplamento, necessário arranjar outra solução
        System.Drawing.Image RenderizarPagina();
    }
}
