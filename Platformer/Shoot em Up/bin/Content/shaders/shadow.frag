uniform float exposure = 0.5;
uniform float decay = 0.5;
uniform float density = 0.5;
uniform float weight = 0.5;
uniform vec2 lightPosition = vec2(0.5,1);
uniform sampler2D texture;
const int NUM_SAMPLES = 100 ;

void main()
{	
	vec2 deltaTextCoord = vec2( gl_TexCoord[0].st - lightPosition.xy );
	vec2 textCoo = gl_TexCoord[0].st;
	deltaTextCoord *= 1.0 /  float(NUM_SAMPLES) * density;
	float illuminationDecay = 1.0;


	for(int i=0; i < NUM_SAMPLES ; i++)
	{
			 textCoo -= deltaTextCoord;
			 vec4 sample = texture2D(texture, textCoo );
		
			 sample *= illuminationDecay * weight;

			 gl_FragColor += sample;

			 illuminationDecay *= decay;
	 }
	 gl_FragColor *= exposure;
}