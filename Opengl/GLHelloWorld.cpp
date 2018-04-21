#include<glut\GL\glut.h>

void DoDisplay()
{
	glClear(GL_COLOR_BUFFER_BIT);

	glBegin(GL_TRIANGLES);
	glVertex2f(0.0, 0.5);
	glVertex2f(-0.5, -0.5);
	glVertex2f(0.5, -0.5);
	glEnd();
	glFlush();
}

void main()
{
	glutCreateWindow("OpenGL");
	glutDisplayFunc(DoDisplay);
	glutMainLoop();
}