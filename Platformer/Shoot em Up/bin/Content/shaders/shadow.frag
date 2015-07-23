uniform float exposure = 0.5f;
uniform float decay = 0.5f;
uniform float density = 0.5f;
uniform float weight = 0.5f;
uniform vec2 lightPosition = vec2(0.7f, 0.7f);
uniform sampler2D texture;
const int NUM_SAMPLES = 20 ;

void main()
{	
	vec2 deltaTextCoord = vec2( gl_TexCoord[0].st - lightPosition.xy );
	vec2 textCoo = gl_TexCoord[0].st;
	deltaTextCoord *= 1.0f /  float(NUM_SAMPLES) * density;
	float illuminationDecay = 1.0f;


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