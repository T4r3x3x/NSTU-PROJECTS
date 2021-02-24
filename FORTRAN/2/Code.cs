        program main
        implicit none 
        real min_x, max_x, step_x, min_y, max_y, step_y, Func,i, j
        integer steps_x, steps_y
        
        OPEN (1,FILE='C:\WATCOM\Output.txt') 
        
        print *, 'Enter min x'
        read *, min_x
        print *, 'Enter max x'
        read *, max_x
        print *, 'Enter step x'
        read *, step_x
        print *, 'Enter min y'
        read *, min_y
        print *, 'Enter max y'
        read *, max_y
        print *, 'Enter step y'
        read *, step_y
        
        steps_x = (max_x - min_x) / step_x
        steps_y = (max_y - min_y) / step_y
        write(1,'(A$)') '           '
        do i = min_x, max_x, step_x
            write(1,'(f11.5$)') i
        end do 
        write(1,*) ' '
        do i = min_y, max_y, step_y
            write(1,'(f11.5$)') i
            do j = min_x, max_x, step_x
                write (1, '(f11.5$)') Func(i,j)     
            end do
            write (1,*) ' '
        end do
        read *, i
        close (1)
        end
        
        
        function Func(x, y)    
            Func = abs(cos(x)/sin(y))
            return
        end
