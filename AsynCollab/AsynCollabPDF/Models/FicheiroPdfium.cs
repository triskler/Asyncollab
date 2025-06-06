﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfiumViewer;
using static AsynCollabPDF.Interfaces;

/// <summary>
/// Representa um ficheiro PDF utilizando a biblioteca PdfiumViewer. Tem como base a interface IFicheiroPDF, que define os métodos 
/// necessários para converter uma página PDF num stream de imagem.
/// </summary>

namespace AsynCollabPDF.Models
{
    class FicheiroPdfium: IFicheiroPDF
    {
        private PdfDocument doc;

        public FicheiroPdfium(string localizacaoFicheiro)
        {
            doc = PdfDocument.Load(localizacaoFicheiro);
        }

        public Stream ConverterPaginaParaStream(int indexPagina, int altura, int largura)
        { 
            var imagemPagina = doc.Render(indexPagina, altura, largura, true);
            var ms = new MemoryStream();
            imagemPagina.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            return ms;
        }

        public void Dispose()
        {
            doc?.Dispose();
        }

        public int NumeroPaginas => doc?.PageCount ?? 0;

        public PdfDocument Doc => doc;
    }
}
