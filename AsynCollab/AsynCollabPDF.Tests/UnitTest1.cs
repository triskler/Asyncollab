using System;
using System.IO;
using System.Windows.Forms;
using Moq;
using Xunit;
using AsynCollabPDF.Models;

namespace AsynCollabPDF.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void AbrirFicheiro_ReturnsTrue_WhenFileExists()
        {
            var model = new Document();
            var testFilePath = Path.Combine("Test_Files", "Teste.pdf");

            var result = model.AbrirFicheiro(testFilePath);

            Assert.True(result);
        }


        [Fact]
        public void AbrirFicheiro_FiresEvent_WhenFileIsOpened()
        {
            var model = new Document();
            var testFilePath = Path.Combine("Test_Files", "Teste.pdf");
            bool eventFired = false;

            model.FicheiroDisponivel += (sender, args) => eventFired = true;


            var result = model.AbrirFicheiro(testFilePath);

            Assert.True(result);
            Assert.True(eventFired);
        }
    }
}