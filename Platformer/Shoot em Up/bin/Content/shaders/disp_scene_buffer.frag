uniform sampler2D texture;     // Scene buffer

void main()
{
    vec3 color = texture2D( texture, gl_TexCoord[0].xy ).rgb;
	gl_FragColor.rgb = color;
	gl_FragColor.a = 1;
}