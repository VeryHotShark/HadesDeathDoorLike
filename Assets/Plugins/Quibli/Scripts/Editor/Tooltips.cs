using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Quibli {
public static class Tooltips {
    private static readonly Dictionary<string, Dictionary<string, string>> Map =
        new Dictionary<string, Dictionary<string, string>>
        {
            {
                "Quibli/Stylized Lit",
                new Dictionary<string, string>
                {
                    { "Gradient", "Defines color and shading of the object." },
                    {
                        "Shading Offset",
                        "Moves the gradient over the model. It’s a convenience parameter, because this effect can be also made by moving all the stop points in the Gradient Editor."
                    },
                    {
                        "Enable Specular",
                        "Specular highlight adds a glare to the object. It can be used for adding a small sharp ‘metallic’ specular, a matte diffused one or anything in between."
                    },
                    {
                        "Enable Rim",
                        "Toggles a set of Rim parameters. In some cases it can be used as a contouring pseudo-outline effect, it can accentuate the edges of the models on the scene. Rim depends on the main light’s rotation and the normals of the shaded model."
                    },
                    { "Rim Color", "Sets the color of the Rim." },
                    {
                        "Light Align",
                        "Moves the Rim on the model toward the main light (usually it is a Directional Light)."
                    },
                    { "Rim Size", "How much of the model the Rim covers." },
                    { "Rim Edge Smoothness", "How sharply the Rim fades out into the base shading." },
                    {
                        "Enable Height Gradient", "Toggles Height Gradient and opens the parameters for its adjustment."
                    },
                    {
                        "Gradient Color",
                        "Picks the source color of the gradient. The destination color is going to be the same one but transparent."
                    },
                    {
                        "Center X",
                        "The source point on the world-space horizontal axis, from which the Height Gradient is spread."
                    },
                    {
                        "Center Y",
                        "The source point on the world-space vertical axis, from which the Height Gradient is spread."
                    },
                    { "Size", "How spread the Height Gradient is." },
                    {
                        "Gradient Angle",
                        "Rotates the Height Gradient around the Center X and Center Y values in world-space."
                    },
                    {
                        "Enable Vertex Colors",
                        "If enabled, the final shading of the object is multiplied by the mesh’s vertex color values. It is a debug parameter, usually this is not used for changing the look."
                    },
                    {
                        "Albedo",
                        "The input for a diffuse texture. Select the texture by clicking on the Select texture slot."
                    },
                    {
                        "Detail Map",
                        "The input for a kind of diffuse texture. This one has two additional blending modes, which is useful for adding some kind of details into the material."
                    },
                    { "Bump Map", "The input for normal maps." },
                    {
                        "Light Color Impact",
                        "Defines how much of an influence the main light’s color has onto the material. Having this parameter allows you to add a night/day/morning/sunset feel to the scene. By automating the light’s color it is possible to achieve the day cycle effect."
                    },
                    {
                        "Receive Shadows",
                        "Once enabled, the material will receive the shadows cast from itself and other objects."
                    },
                    { "Override Realtime Shadow", "Toggles changing the default shadow parameters." },
                    {
                        "Shadow Attenuation Remap",
                        "This range slider is a multi-tool, which can control the tightness, intensity and the scale of the cast shadow. Drag the left and right brackets of the range slider to tighten up or loosen down the shadow edges, move the slider by clicking and dragging its center in order to adjust the intensity."
                    },
                    {
                        "Shadow Color",
                        "Sets the color of the received shadow. If Shadow Color’s Alpha is set to 0 in the color chooser, you’ll get Unity’s native shadows. In other words, Alpha influences the Shadow Color’s impact."
                    },
                    {
                        "Shadow Occlusion",
                        "Mask received Unity shadows in areas where normals face away from the light. Useful to remove shadows that 'go through' objects."
                    },
                    {
                        "Override Baked GI",
                        "If the scene has baked global illumination using either lightmaps or light probes, this toggles the Baked Light Lookup gradient below."
                    },
                    {
                        "Baked Light Lookup",
                        "Remaps the values of Unity’s global illumination to a custom gradient. The mapping is defined by the luminance of the original GI value — darker values map to the left of the gradient and brighter values map to the right. This allows to creatively change the atmosphere of lightmapped scenes by changing only this gradient, e.g. setting the gradient to red/purple can give the scene a neon sunset look."
                    },
                    {
                        "Override Light Direction",
                        "Sets custom light rotation from the main light (usually Directional Light) and lets you control the lighting/shading positions manually and independently per material."
                    },
                    {
                        "Surface Type",
                        "If Transparent Surface Type is selected, the Blend Mode menu becomes available with the following Blend Mode options: Alpha, Premultiply, Additive and Multiply."
                    },
                    { "Render Faces", "Determines what faces to render. The three options are Both, Front, Back." },
                    { "Alpha Clipping", "Discards pixels based on the Albedo texture's alpha channel." },
                    { "Threshold", "The minimum alpha in the Albedo texture to render a pixel." },
                    {
                        "Enable GPU Instancing",
                        "Uses GPU Instancing to render multiple copies of the mesh at once. More information in Unity’s documentation."
                    },
                }
            },
            {
                "Quibli/Foliage",
                new Dictionary<string, string>
                {
                    { "Shading Gradient", "Defines color and shading of the object." },
                    {
                        "Shape Texture",
                        "Defines the overall shape of each foliage particle. Only alpha channel is used."
                    },
                    { "[t]Alpha Clip", "Threshold at which the 'Shape Texture' is cut off." },
                    {
                        "Fill Texture",
                        "A detail map that influences shading within each particle. Only alpha channel is used."
                    },
                    { "[t]Fill Impact", "The degree to which 'Fill Texture' values influence the shading." },
                    { "[t]Fill Scale", "The scale of sampling 'Fill Texture'." },
                    {
                        "Offset Along Normal",
                        "Positive values 'inflate' the model, while negative values make it more compact."
                    },
                    {
                        "[Header]Fresnel",
                        "Fresnel appears in regions where the surface is observed at an oblique angle. It is similar to translucency or 'rim' around objects."
                    },
                    { "[t]Fresnel Power", "Defines fresnel visibility." },
                    { "[t]Fresnel Color", "Color of the fresnel regions." },
                    {
                        "[t]Fresnel Sharpness",
                        "Size of the transition into the fresnel regions. High values result in a visible fresnel edge."
                    },
                    { "Shadow Strength", "Defines visibility of received Unity shadows." },
                    { "Wind", "Toggles vertex and UV motion that simulates wind going through the foliage." },
                    {
                        "[t]Wind Direction",
                        "World-space vector in which the wind blows. The length of the vector is ignored."
                    },
                    { "[t]Wind Speed", "The speed of vertex and UV motion." },
                    { "[t]Wind Turbulence", "The randomness of the wind motion." },
                    { "[t]Wind Strength", "The amplitude of the wind motion." },
                    {
                        "Billboard Scale",
                        "Rotates the foliage particles towards the camera and scales them depending on the 'Billboard Rotation' parameter."
                    },
                    {
                        "Billboard Rotation",
                        "Defines how 'Billboard Scale' is applied.\n" + "- Nothing: Scale is ignored.\n" +
                        "- Each Face: Scales each face separately to face the camera (based on UV).\n" +
                        "- Whole Object: Rotates the object towards the camera and scales it based on vertex positions."
                    },
                    {
                        "Billboard Face Camera Position",
                        "If 'Billboard Rotation' is enabled, the billboard will face the camera position. " +
                        "Otherwise they face the camera plane. This makes billboards look nicer when camera rotates but is more expensive to render."
                    }
                }
            },
            {
                "Quibli/Grass",
                new Dictionary<string, string>
                {
                    { "Base Map", "Single channel texture with alpha defining the shape of grass leaves." },
                    { "[t]Alpha Clip", "Threshold at which the 'Base Map' alpha channel is cut off." },
                    {
                        "Top Color",
                        "The color of the upper part of the grass blade. This color and the color from the 'Bottom Color' parameter are interpolated."
                    },
                    {
                        "Bottom Color",
                        "The color of the lower part of the grass blade. This color and the color from the 'Top Color' parameter are interpolated."
                    },
                    {
                        "Emission",
                        "Color added to the final grass shading value. Useful to adjust the look of the whole material, including wind, gusts, etc."
                    },
                    {
                        "Shadow Strength",
                        "Controls intensity of the shadow coming from the main light of the scene. The value of 0 results in ignoring shadows, the value of 1 results in completely black color in the shadowed regions."
                    },
                    { "[t]Wind Speed", "How fast the material displaces the mesh vertices." },
                    {
                        "[t]Wind Intensity",
                        "Amount of the object's deviation from its initial position, i.e. how strong the movement is."
                    },
                    {
                        "[t]Wind Frequency",
                        "Scale of the sway intervals. Higher values result in denser, more fine-grained noise."
                    },
                    { "[t]Wind Direction", "General direction of the wind motion around Y axis." },
                    {
                        "[t]Wind Turbulence",
                        "Sets the amount of noise that introduces non-linearity to the object’s motion."
                    },
                    { "[t]Gust Intensity", "The strength of the wind blasts." },
                    { "[t]Gust Frequency", "How long or short the intervals of the gusts are." },
                    { "[t]Gust Speed", "How fast the gust-influenced parts of the grass are moving." },
                    { "[t]Gust Color", "When a gust occurs, the influenced grass can change its color to this value." },
                    {
                        "Patches",
                        "Patches control the color non-linearity within a single grass material. When you set the regular colors of the grass, the whole material can sometimes look a bit plain due to the fact that all the grass is equally colored. Introducing slight (or more obvious) random differences in color tint can liven up the look of the grass presentation."
                    },
                    {
                        "[t]Patches Color",
                        "Defines the target color, to which the patches change their tint. Using the following parameters it is possible to make that change abrupt or more gradual."
                    },
                    {
                        "[t]Patches Threshold",
                        "Controls ratio between the area of patches and the rest of the grass. Increasing this value adds new patches."
                    },
                    {
                        "[t]Patches Scale",
                        "Controls the density of the internal noise map where the randomness of the patches distribution comes from. Effectively, this sets the size of all patches."
                    },
                    {
                        "[t]Patches Blurriness",
                        "Controls how abrupt or gradual the color change is from normal colors to the color set in the 'Patches Color' parameter."
                    },
                    {
                        "[t]Patches Offset",
                        "Moves the noise map by the axis. This noise map is where the randomness of the patches comes from."
                    },
                }
            },
            {
                "Quibli/Cloud3D", new Dictionary<string, string>
                {
                    { "Shading Gradient", "Defines the colors of the cloud." },
                    {
                        "Shading Offset",
                        "Moves the gradient over the model. It’s a convenience parameter, this effect can be also made by moving all the stop points in the Gradient Editor."
                    },
                    { "Alpha Threshold", "Sets how much to cut out from the 'Shape Texture'." },
                    { "Billboard Scale", "Rotates each face toward the camera and scales it in view space." },
                    {
                        "Offset Along Normal",
                        "Slides the particles along the normals. Visually it ‘inflates’ or ‘deflates’ the mesh."
                    },
                    { "Shadow Strength", "Controls the visibility of received Unity shadows." },
                    {
                        "Billboard Face Camera Position",
                        "If enabled, the billboard will face the camera position. " +
                        "Otherwise they face the camera plane. This makes billboards look nicer when camera rotates but is more expensive to render."
                    },
                }
            },
            {
                "Quibli/Cloud2D",
                new Dictionary<string, string>
                {
                    { "Main Gradient", "Defines the main colors of a cloud." },
                    {
                        "Geometry Gradient",
                        "Grayscale gradient that does not impact any color work but rather the shape of the cloud, more precisely, the fading out of the contour of a cloud into transparency. White color is fully visible (opaque), black is fully invisible (transparent)."
                    },
                    { "Edge Distortion", "Randomizes the contour of a cloud using an internal noise map." },
                    {
                        "Base Vertical Offset",
                        "Moves a cloud upwards and downwards in an exponential way — it stretches the cloud near the extreme values of this parameter."
                    },
                    {
                        "[Header]Shadow",
                        "This section responsible for controlling the shadowed region of the cloud. Usually this is the lower part of the object that does not receive direct sunlight."
                    },
                    {
                        "[Shadow][t]Color",
                        "Sets the color of the shadowed part of a cloud. For example, you can use red-ish color for a sunset look."
                    },
                    { "[Shadow][t]Amount", "Sets how visible the shadow is." },
                    { "[Shadow][t]Distortion", "Controls the randomness of the shadow edge." },
                    { "[Shadow][Vector2][t]Center", "Sets the coordinates of the center of the shadow." },
                    {
                        "[Shadow][Vector2][t]Range",
                        "Sets how spread out the shadow is in horizontal (X) and vertical (Y) directions."
                    },
                    { "Edge Highlight", "The color of a halo visible around the cloud." },
                    { "Opacity", "Sets how visible the 'Edge Highlight' is." },
                    { "Height Gradient Strength", "Controls vertical shading of the cloud using the 'Main Gradient'." },
                    {
                        "[Vector2]Random Offset",
                        "Scrolls the internal displacement map of a cloud. Can be used as a parameter to change the randomness seed and form unique clouds."
                    },
                    { "[Vector2]Offset Speed", "Sets the speed of geometry motion of the clouds." },
                    {
                        "Object Position Impact",
                        "Determines how cloud reacts to its position change. At the value of 0 the cloud doesn't change its shape while being moved. At the value of 1 a slight position change has a great impact on the cloud's shape."
                    },
                    {
                        "[Header]Geometry Density",
                        "Parameters of the small-, medium- and large-scale details of the cloud shape."
                    },
                    {
                        "[Geometry Density][t]Large",
                        "Changes the size of large-scale details in the internal displacement map. Increasing the value makes the details smaller; decreasing the values makes the details larger."
                    },
                    {
                        "[Geometry Density][t]Medium",
                        "Changes the size of medium-scale details in the internal displacement map."
                    },
                    {
                        "[Geometry Density][t]Small",
                        "Changes the size of small-scale details in the internal displacement map."
                    },
                    {
                        "Face Camera",
                        "Billboard aka ‘always look into the camera’ effect, which is helpful in situations when the clouds may be approachable by the camera and not desired to be seen from an angle."
                    },
                }
            }
        };

    public static string Get(MaterialEditor editor, string displayName) {
        var material = editor.target as Material;
        Debug.Assert(material != null, nameof(material) + " != null");
        var shaderHasTooltips = Map.TryGetValue(material.shader.name, out var tooltips);
        if (!shaderHasTooltips) return null;
        var propertyHasTooltip = tooltips.TryGetValue(displayName, out var tooltip);
        if (!propertyHasTooltip) return null;
        return tooltip;
    }
}
}