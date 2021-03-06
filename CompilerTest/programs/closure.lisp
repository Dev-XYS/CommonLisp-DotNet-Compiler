(defun make-closure ()
  (let (x)
    (lambda (cmd val)
      (if (eq cmd 'get)
	  x
	  (if (eq cmd 'set)
	      (setq x val)
	      nil)))))
(let ((c1 (make-closure)) (c2 (make-closure)))
  (c1 'set 5)
  (c2 'set 7)
  (writeln (c1 'get 0))
  (writeln (c2 'get 0)))
