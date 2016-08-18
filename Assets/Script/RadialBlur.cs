using UnityEngine;

//[ExecuteInEditMode]
public class RadialBlur : MonoBehaviour
{
	public Shader rbShader;
	public Material screenMaterial;

	float blurStrength = 0.0f;
	float blurWidth = 0.8f;

	void OnRenderImage(RenderTexture source, RenderTexture dest)
	{
		if (blurStrength > 0.0f)
		{
			screenMaterial.SetFloat("_BlurStrength", blurStrength);
			screenMaterial.SetFloat("_BlurWidth", blurWidth);
			Graphics.Blit(source, dest, screenMaterial);
		}
	}

	void Update()
	{
		if(blurStrength > 0.0f)
		{
			blurStrength -= Time.deltaTime * 2.0f / 0.8f;
			if (blurStrength < 0.0f)
				blurStrength = 0.0f;
		}
	}

	public void Show()
	{
		blurStrength = 2.0f;
	}
}
