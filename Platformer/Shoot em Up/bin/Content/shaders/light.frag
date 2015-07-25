
uniform vec2 lightPosition = vec2(0.5f, 0.7f);
uniform sampler2D texture;

const int NUM_SAMPLES = 25;
const float exposure = 0.5f;
const float density = 0.5f;
const float weight = 0.1f;

void main() {	
    vec2 texCrd = gl_TexCoord[0].st;
    vec2 deltaTexCrd = vec2(texCrd - lightPosition.xy);
    deltaTexCrd *= 1.f / float(NUM_SAMPLES) * density;

    for (int i = 0; i < NUM_SAMPLES; i++) {
        texCrd -= deltaTexCrd;
        gl_FragColor += texture2D(texture, texCrd) * weight;
    }

    gl_FragColor *= exposure;
	gl_FragColor.a = (gl_FragColor.r + gl_FragColor.g + gl_FragColor.b) / 3.0f;
}