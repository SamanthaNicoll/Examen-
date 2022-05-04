﻿using Common;
using Domain.Entities;
using Infraestructure.OpenWeatherClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExamenWeather
{
    public partial class Form1 : Form
    {
        string ciudad;
        double x, y;
        long dt = DateTimeOffset.Now.ToUnixTimeSeconds();
        //public OpenWeather openWeather;
        public WeatherForeCast.ForeCastInfo wfc;
        public OpenWeatherWeb opw;
        public List <coordenadas> cd;
        public Form1()
        {
            opw = new OpenWeatherWeb();
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                ciudad = cmbCity.SelectedItem.ToString();
              
                Task.Run(Request).Wait();
                
                x = cd[0].lat;
                y = cd[0].lon;
                Task.Run(Request2).Wait();
                if (wfc == null)
                {
                    throw new NullReferenceException("Fallo al obtener el objeto OpeWeather.");
                }
                double temp = wfc.current.temp - 273.15;
                WeatherPanel weatherPanel = new WeatherPanel();
                weatherPanel.x = x;
                weatherPanel.y = y;
                weatherPanel.lblCity.Text = ciudad;
                weatherPanel.lblTemp.Text = (int)temp + "C";
                weatherPanel.lblWeather.Text = wfc.current.weather[0].main;
                weatherPanel.WeatherIcon.ImageLocation = $"{AppSettings.ApiIcon}" + wfc.current.weather[0].icon + ".png";
                flpContent.Controls.Add(weatherPanel);

            }
            catch (Exception)
            {

               
            }
        }

        public async Task Request()
        {
            cd = await opw.GetLatLong(ciudad);
        }
        public async Task Request2()
        {
            wfc = await opw.GetWeatherByGeo(x,y,dt);
        }

        private void flpContent_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void cmbCity_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string fileName = "NicaraguaCitys.json";
            string json = File.ReadAllText(fileName);
            List<Citys> Ciudades = new List<Citys>();
            Ciudades = JsonConvert.DeserializeObject<List<Citys>>(json);
            foreach (Citys c in Ciudades)
            {
                cmbCity.Items.Add(c.City);
            }
        }
    }
    }

