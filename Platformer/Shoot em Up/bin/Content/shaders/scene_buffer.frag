uniform sampler2D texture;     // Sprite texture
uniform sampler2D rt_scene;    // Scene render texture, which is what the shader is drawing to. I pass this in here so I can mix without setting the alpha
uniform float r_depth;         // Fixed depth value for every sprite
void main()
{
    vec4 color = texture2D( texture, gl_TexCoord[0].xy );
    vec4 sceneBuffer = texture2D( rt_scene, gl_TexCoord[0].zw ); // gl_TexCoord[0].zw are screen space texture coords
   
    gl_FragColor.rgb = mix( sceneBuffer.rgb, color.rgb, color.a );
    gl_FragColor.a = mix( sceneBuffer.a, r_depth, color.a );
}