using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoilChangeManager : MonoBehaviour
{ 
    public ParticleSystem fallingSoil;
    private SoilTypes soil;
    private ParticleSystem.MainModule settings;

    // Start is called before the first frame update
    void Start()
    {
        settings = fallingSoil.GetComponent<ParticleSystem>().main;
    }

    public void SetSandySoil() { 
        soil = SoilTypes.Sandy;
        ChangeSoil();
    }

    public void SetLoamySoil()
    {
        soil = SoilTypes.Loamy;
        ChangeSoil();
    }

    public void SetSiltySoil()
    {
        soil = SoilTypes.Silty;
        ChangeSoil();
    }

    public void SetClaySoil()
    {
        soil = SoilTypes.Clay;
        ChangeSoil();
    }

    /*public SoilTypes GetSoilType(string name) {
        string soilName = name.ToLower();
        if (soilName.Equals("sand"))
            return SoilTypes.Sandy;
        else if (soilName.Equals("silt"))
            return SoilTypes.Silty;
        else if (soilName.Equals("loam"))
            return SoilTypes.Loamy;
        else
            return SoilTypes.Clay;
    }*/

    public SoilTypes GetSoilType() 
    {
        return soil;
    }

    // Update is called once per frame
    private void ChangeSoil()
    {
        Color newColor;
        switch (soil) 
        {
            case SoilTypes.Sandy:
                newColor = new Color(0.69f, 0.51f, 0.10f, 1f);
                settings.startColor = newColor;
                break;
            case SoilTypes.Silty:
                newColor = new Color(0.18f, 0.16f, 0.13f, 1f);
                settings.startColor = newColor;
                break;
            case SoilTypes.Loamy:
                newColor = new Color(0.26f, 0.22f, 0.13f, 1f);
                settings.startColor = newColor;
                break;
            default:
                settings.startColor = Color.black;
                break;
        }
    }
}
