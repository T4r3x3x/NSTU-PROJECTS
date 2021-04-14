        program main
        
        real min_x, max_x, step_x, min_y, max_y, step_y, i, j,x,y
        real temp
        integer steps_x, steps_y
        logical invisible 
        OPEN (1,FILE='C:\Users\pmi-b9205\Desktop\Input.txt')
        OPEN (2,FILE='C:\Users\pmi-b9205\Desktop\Output.txt') 
        
        read (1,*) min_x 
        read (1,*) max_x
        read (1,*) step_x
        read (1,*) min_y
        read (1,*) max_y
        read (1,*) step_y
                
        steps_x = (max_x-min_x)/step_x
        steps_y = (max_y-min_y)/step_y
        
        write(2,'(A$)') '             | '
        
         do i = 0, steps_x            
            if(invisible(x,sum_rump(min_x,i*step_x))) then    
                x = sum_rump(min_x,i*step_x)          
                write(2,'(e12.4$,(A$))') x, ' | '                  
            end if    
            x = sum_rump(min_x,i*step_x)               
        end do
        
        if(x .LT. max_x) then
            write(2,'(e12.4$, (A$))') max_x, ' | '
        end if     
        
        write (2,*) ' '
        
        do i = 0, steps_y
             if(invisible(y,sum_rump(min_y,i*step_y))) then 
                y = sum_rump(min_y,i*step_y)
                write(2,'(e12.4$,(A$))') y, ' | '
             end if
            y = sum_rump(min_y,i*step_y)
            
            do j = 0, steps_x
                if(invisible(x,sum_rump(min_x,i*step_x))) then 
                    x = sum_rump(min_x,j*step_x)                
                    call func(x,y)              
                end if
                x = sum_rump(min_x,j*step_x)
            end do 
            
            if(x .LT. max_x) then
                call Func(max_x,y)
            end if
            
            write (2,*) ' '                            
        end do
        
        if(y .LT. max_y) then
                    write(2,'(e12.4$,(A$))') max_y, ' | '
                    y = max_y
                    do j = 0, steps_x
                        if(invisible(x,sum_rump(min_x,i*step_x))) then 
                            x = sum_rump(min_x,j*step_x)                
                            call func(x,y)              
                        end if
                        x = sum_rump(min_x,j*step_x)                       
                    end do 
            
                    if(x .LT. max_x) then
                        call Func(max_x,y)
                    end if
            end if            
        end 
       
       
       
        REAL FUNCTION sum_rump(X)
            real x,s,c,e, twosum
            s = 0.0
            c = 0.0
            s = twosum(x,s,e)
            sump_rump = s + c
            return
        end       
       
        REAL FUNCTION TWOSUM(ERROR, A, B)  
      
        IMPLICIT NONE
        REAL A, B, ERROR
        REAL s
        s = a + b
       
      
        END
        
        
        subroutine Func(x, y)             
            real x,y 
            if(cos(y) .NE. 0) then
                write (2, '(e12.4$, A$)') abs(sin(x)/cos(y)),' | ' 
            else
                write(2, '(A$)') '  div. by 0  | '
            end if             
            return
        end
        
               
                  
                 logical function Invisible(x,y)
                 implicit none
                 real x,y
                 integer a,b
                 call exp (x,a)
                 call exp (y,b)
                 print *, a
                 print *, b
                 if(a-b .EQ. 0) then
                    Invisible = .FALSE.
                    return
                 else
                    Invisible = .TRUE.                    
                    return
                  end if
                  end
                
                
                
                
                subroutine Exp(x, res)
                implicit none
                real x, k, temp
                integer i, j, res
                temp = x
                j = 0
                k = 10
                
                if( x .EQ. 1) then 
                j = 3
                else if( x .EQ. 10) then  
                j = 2 
                else if( x .EQ. 100) then 
                j = 1 
                else 
                
                
                if(x .GE. 10000) then 
                    j = x/10000
                    k = 0.1
                    if(x/10000 .NE. 0) then
                        j = j + 1
                    end if
                else if (x .LT. 1000) then
                    j = 1000/x   
                    k = 10
                end if
                
                call Times(j)
                
                end if 
                
                
                do i = 1, j
                    temp = temp * k
                end do  
                res = int(temp)
                end
                
                
                subroutine Times(j)
                implicit none
                integer i, k, j                
                k = j
                if(j .NE. 0) then
                j = 1
                else 
                j=0
                end if
                
                do i = 1, 10
                    if(k/10 .EQ. 0) then 
                        return
                    else
                        k = k / 10
                        j = j +1
                    end if
                end do
                
                end
