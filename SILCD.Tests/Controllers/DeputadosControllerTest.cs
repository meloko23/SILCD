using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SILCD.Controllers;
using System.Web.Mvc;
using SILCD.Repository.Abstract;
using Moq;
using SILCD.Models;
using SILCD.Enum;
using System.Collections;

namespace SILCD.Tests.Controllers {
    [TestClass]
    public class DeputadosControllerTest {

        [TestMethod]
        public void Index() {
            // moq object
            Mock<IDeputadosRepository> mock = new Mock<IDeputadosRepository>();
            mock.Setup(d => d.List).Returns(new DeputadoViewModel[] {
                                                new DeputadoViewModel { IdeCadastro = 74210, 
                                                                        Condicao="Titular", 
                                                                        Matricula = "85", 
                                                                        IdParlamentar = 520939, 
                                                                        Nome = "JOSÉ SARNEY FILHO", 
                                                                        NomeParlamentar="SARNEY FILHO", 
                                                                        UrlFoto = "http://www.camara.gov.br/internet/deputado/bandep/74210.jpg", 
                                                                        Sexo= "masculino", 
                                                                        Uf="MA", 
                                                                        Partido="PV", 
                                                                        Gabinete="202", 
                                                                        Anexo = "4", 
                                                                        Telefone="3215-5202", 
                                                                        Email="dep.sarneyfilho@camara.gov.br"},
                                    });

            // controller
            DeputadosController controller = new DeputadosController(mock.Object);


            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    
    }
}
