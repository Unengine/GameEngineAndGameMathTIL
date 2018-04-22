//표준 헤더 포함
#include<stdio.h>
#include<stdlib.h>
//glew 먼저 포함. gl.h, glfw.h 포함하기 전 먼저 해야함.
#include<GL/glew.h>
//키보드, 마우스를 다루는 glfw. 사실 3d 계산 라이브러리다.
#include<GLFW\glfw3.h>
//glm 포함
#include<glm/glm.hpp>
//충돌 방지
using namespace glm;

int main()
{
	if (!glfwInit())
	{
		fprintf(stderr, "GLFW 초기화 실패");
		return -1;
	}

	glfwWindowHint(GLFW_SAMPLES, 4); // 4x 안티에일리어싱
	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3); // OpenGL 3.3 을 쓸 겁니다
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
	glfwWindowHint(GLFW_OPENGL_FORWARD_COMPAT, GL_TRUE); // MacOS 가 기분 좋아짐; 꼭 필요한 부분은 아님
	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE); //옛날 OpenGL은 원하지 않아요

	 // 새창을 열고, OpenGL 컨텍스트를 생성
	GLFWwindow* window; // (후술되는 코드를 보면, 이 변수는 전역(Global)입니다.)
	window = glfwCreateWindow(1024, 768, "Tutorial 01", NULL, NULL);
	if (window == NULL) {
		fprintf(stderr, "GLFW 윈도우를 여는데 실패했습니다. Intel GPU 를 사용한다면, 3.3 지원을 하지 않습니다. 2.1 버전용 튜토리얼을 시도하세요.\n");
		glfwTerminate();
		return -1;
	}

	glfwMakeContextCurrent(window); // GLEW 초기화
	glewExperimental = true; // 코어 프로파일을 위해 필요함
	if (glewInit() != GLEW_OK) {
		fprintf(stderr, "Failed to initialize GLEW\n");
		return -1;
	}

	glfwSetInputMode(window, GLFW_STICKY_KEYS, GL_TRUE);

	do {
		// 아무것도 그리지 않습니다. 두번째 튜토리얼에서 만나도록 하죠!

		// 버퍼들을 교체
		glfwSwapBuffers(window);
		glfwPollEvents();

	} // 만약 ESC 키가 눌러졌는지 혹은 창이 닫혔는지 체크 체크
	while (glfwGetKey(window, GLFW_KEY_ESCAPE) != GLFW_PRESS &&
		glfwWindowShouldClose(window) == 0);
}
