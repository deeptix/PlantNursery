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

    public SoilTypes GetSoilType() 
    {
        return soil;
    }

    public Color GetSoilColor(SoilTypes s) 
    {
        Color newColor;
        switch (s) 
        {
            case SoilTypes.Sandy:
                newColor = new Color(0.69f, 0.51f, 0.10f, 1f);
                break;
            case SoilTypes.Silty:
                newColor = new Color(0.52f, 0.49f, 0.42f, 1f);
                break;
            case SoilTypes.Loamy:
                newColor = new Color(0.26f, 0.22f, 0.13f, 1f);
                break;
            default:
                newColor = Color.black;
                break;
        }
        return newColor;
    }

    private void ChangeSoil()
    {
        settings.startColor = GetSoilColor(soil);
    }
}
