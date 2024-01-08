﻿// Autogenerated models

export class SolarPanelParameters
{
    year: number;
    month: number;
    day: number;
    latitude: number;
    panelTilt: number;
    panelPower: number;
    nOCT: number;
    ambientTemperature: number;
    panelEfficiency: number;
    panelArea: number;
    powerTemperatureCoefficient: number;

    constructor(year: number = null,month: number = null,day: number = null,latitude: number = null,panelTilt: number = null,panelPower: number = null,nOCT: number = null,ambientTemperature: number = null,panelEfficiency: number = null,panelArea: number = null,powerTemperatureCoefficient: number = null,)
    {
        this.year = year;
        this.month = month;
        this.day = day;
        this.latitude = latitude;
        this.panelTilt = panelTilt;
        this.panelPower = panelPower;
        this.nOCT = nOCT;
        this.ambientTemperature = ambientTemperature;
        this.panelEfficiency = panelEfficiency;
        this.panelArea = panelArea;
        this.powerTemperatureCoefficient = powerTemperatureCoefficient;
    }
}