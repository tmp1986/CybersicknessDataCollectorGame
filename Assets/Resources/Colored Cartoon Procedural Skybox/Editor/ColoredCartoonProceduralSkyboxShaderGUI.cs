using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ColoredCartoonProceduralSkyboxShaderGUI : ShaderGUI 
{
	public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
	{
		OnGUI_DrawShaderProperty("_Tint", "Tint", materialEditor, properties);
		OnGUI_DrawShaderProperty("_NightTint", "NightTint", materialEditor, properties);
		OnGUI_DrawShaderProperty("_Exposure", "Exposure", materialEditor, properties);
		OnGUI_DrawShaderProperty("_Rotation", "Rotation", materialEditor, properties);

		BeginBox();
		{
			EditorGUILayout.LabelField("Sun Disc");
			OnGUI_DrawShaderProperty("_SunDiscColor", "Color", materialEditor, properties);
			OnGUI_DrawShaderProperty("_SunDiscMultiplier", "Multiplier", materialEditor, properties);
			OnGUI_DrawShaderProperty("_SunDiscExponent", "Exponent", materialEditor, properties);
		}
		EndBox();

		BeginBox();
		{
			EditorGUILayout.LabelField("Sun Halo");
			OnGUI_DrawShaderProperty("_SunHaloColor", "Color", materialEditor, properties);
			OnGUI_DrawShaderProperty("_SunHaloExponent", "Exponent", materialEditor, properties);
			OnGUI_DrawShaderProperty("_SunHaloContribution", "Contribution", materialEditor, properties);
		}
		EndBox();

		BeginBox();
		{
			EditorGUILayout.LabelField("Horizon Line");
			OnGUI_DrawShaderProperty("_HorizonLineColor", "Color", materialEditor, properties);
			OnGUI_DrawShaderProperty("_HorizonLineExponent", "Exponent", materialEditor, properties);
			OnGUI_DrawShaderProperty("_HorizonLineContribution", "Contribution", materialEditor, properties);
		}
		EndBox();

		BeginBox();
		{
			EditorGUILayout.LabelField("Sky Gradient");
			OnGUI_DrawShaderProperty("_SkyGradientTop", "Top", materialEditor, properties);
			OnGUI_DrawShaderProperty("_SkyGradientBottom", "Bottom", materialEditor, properties);
			OnGUI_DrawShaderProperty("_SkyGradientExponent", "Exponent", materialEditor, properties);
		}
		EndBox();
	}

	public static bool OnGUI_DrawShaderProperty(string propertyName, string label, MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        MaterialProperty mp = FindProperty(propertyName, properties, false);
        if(mp != null)
        {
            materialEditor.ShaderProperty(mp, label);
        }
        return mp != null;
    }

	public static void BeginBox()
	{
		EditorGUILayout.BeginVertical(GUI.skin.GetStyle("Box"));
		EditorGUILayout.Space();
	}

	public static void EndBox()
	{
		EditorGUILayout.Space();
		EditorGUILayout.EndVertical();
	}
}
