uniform sampler2D rt_scene;    // Scene render texture, which is what the shader is drawing to. I pass this in here so I can mix without setting the alpha

void main()
{
    vec4 sceneBuffer = texture2D( rt_scene, gl_TexCoord[0].xy );
   
    gl_FragColor.r = gl_FragColor.g = gl_FragColor.b = 1 - sceneBuffer.a;
	gl_FragColor.a = sceneBuffer.a;
}