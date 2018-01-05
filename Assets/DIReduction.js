//#pragma strict
var mds : boolean = true;

function chg() {
  mds = !mds;
  start();
}

function start() {
  if(mds) {
//    computeMDS();
  } else {
    PCAClicked();
  }
}
function update(){

}
var tmax=0;
var b =[[]];



function computeMDS( ) {





  var max = Mathf.Max( b[0]) ;
  for(var i :int =1;i<b.length;i++){
  
	if(max<Mathf.Max( b[i])){
		max=Mathf.Max( b[i]);
	}
	
  }
  for(i =0;i<b.length;i++){
	for(var x:int =0;x<b[i].length;x++){
	 b[i][x]=b[i][x]/(max);
		
	}
  }
  	if(tmax<max){
		tmax=max;
	}
  var a = mdsjs.convertToMatrix(b, true);
  
  mdsjs.landmarkMDSAsync(a, 2, function(points :int[]) {
    var ctx = document.getElementById("canvas").getContext("2d");
    ctx.clearRect(0, 0, 400, 400);
    ctx.beginPath();
    ctx.rect(0, 0, 400, 400);
    ctx.stroke();
    points.rowsIter(function(row) {
      var x = (row[0] /40000) + 400;
      var y = (row[1] /40000) + 400;
      ctx.fillRect(x - 2, y - 2, 4, 4);
    });
    document.getElementById("mode").innerText = "mds";
    document.getElementById("output").innerText = "" + points;
    document.getElementById("matrix").innerText = "" + a;
  });
}
function PCAClicked(){
  var b= [
    [1, 1, 3],
    [2, 2, 3],
    [3, 8, 26],
	[4, 4, 15],
    [5, 23, 13],
    [600, 45, 12],
	[12, 700, 26],
	[700, 4, 15],
    [5, -1, 67],
	[23, 4, 15],
    [55, -1, 67],
    [-900, -828, -70],
	[55, -1, 66],
    [-900, -825, -70],
	[55, -1, 65],
    [-900, -822, -70],
	[55, -1, 64],
    [-900, -821, -70],
    [578, -90, 34]
  ];
  computePCA(b);
}

function computePCA(b: int[]) {
  var posArr = [];
  var xspread = 180;
  var yspread = 150;

  var q=0;

  a();
  }
  function a() {
  
	  for(var i = 0;i < 1000;i += 1) {
		var rnd = Math.random() * 2 * Math.PI;
		posArr.push([ Math.cos(rnd) * xspread, Math.sin(rnd) * yspread ]);
	  }
	  
	  
	  var max = Math.max.apply(Math, b[0]) ;
	  for(i=1;i<b.length;i++){
	  
		if(max<Math.max.apply(Math, b[i])){
			max=Math.max.apply(Math, b[i]);
		}
		
	  }
	  for(i=0;i<b.length;i++){
		for(x=0;x<b[i].length;x++){
		 b[i][x]=b[i][x]/(max);
			
		}
	  }

	  //work around cast
	  var castingArray = new MultiDim.IntArray(0,0);
	  castingArray=b;
	  b=castingArray;

	  var pos = mdsjs.convertToMatrix(b);


	  mdsjs.pcaAsync(pos, function(pca:int[]) {
		var points = pos.mul(pca);
		var ctx = document.getElementById("canvas").getContext("2d");
		ctx.clearRect(0, 0, 800, 800);
		ctx.beginPath();
		ctx.rect(0, 0, 800, 800);
		ctx.stroke();

		ctx.save();
		ctx.fillStyle = "blue";
		var index=0;
		var start=[];
		pos.rowsIter(function(row :int) {
		  var x :int = (row[0]*200) + 400;
		  var y :int = (row[1]*200) + 400;
		  start.push([x,y]);
		  ctx.fillRect(x - 2, y - 2, 4, 4);
		});
		ctx.restore();

		ctx.save();
		ctx.fillStyle = "orange";
		index=0;
		
		var e=[];
		var end=[];
		points.rowsIter(function(row :int) {
		  index += 1;
		  if(index>3){
			ctx.fillStyle = "red";
		  }
		  e.push(row);
		  var x :int = (row[0]*200) + 400;
		  var y :int = (row[1]* 200) + 400;
		  end.push([x,y]);
	//	  ctx.fillRect(x - 2, y - 2, 4, 4);
		});
		
		for(i=0;i<start.length;i++){
	//		ctx.moveTo(start[i][0],start[i][1]);
	//		ctx.lineTo(end[i][0],end[i][1]);
	//		ctx.stroke();
		}


	//	ctx.restore();
	//	document.getElementById("ratio").innerText = "" + (max);
	//	document.getElementById("mode").innerText = "pca";
	//	document.getElementById("output").innerText = pca + "\n" + points;
	//	document.getElementById("matrix").innerText = "" + pos;
		b=e;
		});

			setTimeout(function()
		{
			a();

		},34);//document.getElementById("myRange").value);
	}









/**


MDSJS.js
* Created by krause on 2014-10-25.
altered by rex 1/2/2018




 
 */

mdsjs = function() {
  var thatMDS = this;

  this.DEBUG = false;
  this.noNaNs = function(arr) {
    for(var ix = 0;ix < arr.length;ix += 1) {
      if(Number.isNaN(arr[ix])) {
        throw new Error("NaN in array");
      }
    }
  };
  this.noZeros = function(arr) {
    for(var ix = 0;ix < arr.length;ix += 1) {
      if(Number.isNaN(arr[ix]) || arr[ix] === 0) {
        throw new Error(arr[ix] === 0 ? "0 in array" : "NaN in array");
      }
    }
  };
  this.onlyPositive = function(arr) {
    for(var ix = 0;ix < arr.length;ix += 1) {
      if(Number.isNaN(arr[ix]) || !(arr[ix] > 0)) {
        throw new Error(!(arr[ix] > 0) ? arr[ix] + " in array" : "NaN in array");
      }
    }
  };

  this.CALL_ASYNC = function(f) {
    setTimeout(f, 0);
  };
  this.getCallDirect = function() {
    var depth = 0;
    return function(f) {
      if(depth > 20) { // prevent stack-overflows
        throw {
          __continuation: f
        };
      }
      if(!depth) {
        var cc = f;
        while(cc) {
          try {
            depth += 1;
            cc();
            cc = null;
          } catch(e) {
            if(!e.__continuation) {
              throw e;
            }
            cc = e.__continuation;
          }
          depth = 0;
        }
      } else {
        depth += 1;
        f();
      }
    };
  };

  this.pca = function(positions) {
    var res;
    thatMDS.pcaAsync(positions, function(mat) {
      res = mat;
    }, thatMDS.getCallDirect());
    return res;
  };
  this.pcaAsync = function(positions, cb, argCall) {
    var call = arguments.length > 2 ? argCall : thatMDS.CALL_ASYNC;
    var centered = positions.colCenter();
    var rows = centered.rows();
    var cols = centered.cols();
    centered.powerIterAsync(function(pca0) {
      var mat = thatMDS.removeComponent(centered, pca0);
      mat.powerIterAsync(function(pca1) {
        var res = centered.createArray(cols, 2);
        for(var ix = 0;ix < cols;ix += 1) {
          res[2*ix + 0] = pca0[ix];
          res[2*ix + 1] = pca1[ix];
        }
        cb(new Matrix(res, cols, 2));
      }, call);
    }, call);
  };
  this.GRAM_SCHMIDT_EPS = 1e-12;
  this.removeComponent = function(mat, comp) {
    // Gram–Schmidt process
    var rows = mat.rows();
    var cols = mat.cols();
    comp.length === cols || console.warn("incompatible size", comp.length, cols);

    proj=function(vec, from, sub, fromSub, len) {
      var res = mat.createArray(1, len);
      var uv = 0;
      var uu = 0;
      for(var ix = 0;ix < len;ix += 1) {
        uv += sub[fromSub + ix] * vec[from + ix];
        uu += sub[fromSub + ix] * sub[fromSub + ix];
      }
      if(Math.abs(uv) < thatMDS.GRAM_SCHMIDT_EPS || Math.abs(uu) < thatMDS.GRAM_SCHMIDT_EPS || !Number.isFinite(uu) || !Number.isFinite(uv)) {
        for(var ix = 0;ix < len;ix += 1) {
          res[ix] = 0;
        }
      } else {
        for(var ix = 0;ix < len;ix += 1) {
          res[ix] = uv / uu * sub[fromSub + ix];
        }
      }
      return res;
    };

    var nextMat = mat.createArray(rows, cols);
    var pos = 0;
    for(var r = 0;r < rows;r += 1) {
      mat.rowIter(r, function(v) {
        nextMat[pos] = v;
        pos += 1;
      });
    }
    for(var r = 0;r < rows;r += 1) {
      var curPos = r * cols;
      thatMDS.normalizeVec(nextMat, curPos, curPos + cols);
      for(var ix = r + 1;ix < rows;ix += 1) {
        var pos = ix * cols;
        var p = proj(nextMat, pos, nextMat, curPos, cols);
        for(var c = 0;c < cols;c += 1) {
          nextMat[pos + c] -= p[c];
        }
      }
    }
    return new Matrix(nextMat, rows, cols);
  };
  this.pcaPositions = function(positions) {
    var pca = thatMDS.pca(positions);
    return positions.mul(pca);
  };
  this.landmarkMDS = function(dist, dims) {
    var res;
    thatMDS.landmarkMDSAsync(dist, dims, function(mat) {
      res = mat;
    }, thatMDS.getCallDirect());
    return res;
  };
  this.landmarkMDSAsync = function(dist, dims, cb, argCall) {

   landmarkMatrix= function (mat) {
      thatMDS.DEBUG && mat.noNaNs();
      var rows = mat.rows();
      var cols = mat.cols();
      var perm = new Uint32Array(rows);
      for(var r = 0;r < rows;r += 1) {
        mat.rowIter(r, function(v, r, c) {
          if(!v) {
            perm[r] = c;
          }
        });
      }
      thatMDS.DEBUG && thatMDS.noNaNs(perm);
      var lm = mat.createArray(rows, rows);
      var pos = 0;
      for(var r = 0;r < rows;r += 1) {
        var mPos = r * cols;
        for(var c = 0;c < cols;c += 1) {
          lm[pos] = mat.getUnsafe(mPos + perm[c]);
          pos += 1;
        }
      }
      thatMDS.DEBUG && thatMDS.noNaNs(lm);
      return new Matrix(lm, rows, rows);
    };

    landmarkResult=function (dist, dims, eigenVecs, eigenVals) {
      var rows = dist.rows();
      var cols = dist.cols();
      var distSq = dist.squareElements();
      thatMDS.DEBUG && distSq.noNaNs();

      var mean = dist.createArray(1, cols);
      for(var c = 0;c < cols;c += 1) {
        distSq.colIter(c, function(v) {
          mean[c] += v;
        });
        mean[c] /= rows;
      }
      thatMDS.DEBUG && thatMDS.noNaNs(mean);

      thatMDS.DEBUG && eigenVecs.noNaNs();
      thatMDS.DEBUG && thatMDS.noZeros(eigenVals);
      var tmp = eigenVecs.createArray(eigenVecs.rows(), eigenVecs.cols());
      var pos = 0;
      for(var r = 0;r < eigenVecs.rows();r += 1) {
        var div = Math.sqrt(Math.abs(eigenVals[r])); // TODO not sure how to handle negative values
        eigenVecs.rowIter(r, function(v) {
          tmp[pos] = v / div;
          pos += 1;
        });
      }
      thatMDS.DEBUG && thatMDS.noNaNs(tmp);

      var positions = dist.createArray(cols, dims);
      pos = 0;
      for(var e = 0;e < cols;e += 1) {
        var m = mean[e];
        var tPos = 0;
        for(var d = 0;d < dims;d += 1) {
          var cur = 0;
          distSq.colIter(e, function(v) {
            cur -= 0.5 * (v - m) * tmp[tPos];
            tPos += 1;
          });
          positions[pos] = cur;
          pos += 1;
        }
      }
      thatMDS.DEBUG && thatMDS.noNaNs(positions);
      return new Matrix(positions, cols, dims);
    };

    var call = arguments.length > 3 ? argCall : thatMDS.CALL_ASYNC;
    var lm = landmarkMatrix(dist);
    var eigenVals = dist.createArray(1, dims);
    lm.squareElements().doubleCenter().scale(-0.5).eigenAsync(eigenVals, function(eigenVecs) {
      cb(landmarkResult(dist, dims, eigenVecs, eigenVals));
    }, call);
  };
  this.normalizeVec = function(vec, f, t) {
    var from = arguments.length > 1 ? f : 0;
    var to = arguments.length > 2 ? t : vec.length;
    var sum = 0;
    for(var i = from;i < to;i += 1) {
      sum += vec[i] * vec[i];
    }
    sum = Math.sqrt(sum);
    if(sum < 1e-30 || !Number.isFinite(sum)) { // don't scale when really small
      return;
    }
    for(var i = from;i < to;i += 1) {
      vec[i] /= sum;
    }
  };
  this.lengthSq = function(vec, f, t) {
    var from = arguments.length > 1 ? f : 0;
    var to = arguments.length > 2 ? t : vec.length;
    var sum = 0;
    for(var i = from;i < to;i += 1) {
      sum += vec[i] * vec[i];
    }
    return sum;
  };
  this.prod = function(vecA, fromA, vecB, fromB, len) {
    var sum = 0;
    var posA = fromA;
    var posB = fromB;
    for(var i = 0;i < len;i += 1) {
      sum += vecA[posA] * vecB[posB];
      posA += 1;
      posB += 1;
    }
    return sum;
  };
  this.xcopy = function(fromVec, fromStart, toVec, toStart, len) {
    var fromPos = fromStart;
    var toPos = toStart;
    for(var i = 0;i < len;i += 1) {
      toVec[toPos] = fromVec[fromPos];
      fromPos += 1;
      toPos += 1;
    }
  };
  this.convertToMatrix = function(arrs, useFloat32) {
    var rows = arrs.length;
    if(!rows) {
      console.warn("invalid dimension (rows)", rows);
      return null;
    }
    var cols = arrs[0].length;
    if(!cols) {
      console.warn("invalid dimension (cols)", cols);
      return null;
    }
    var size = rows * cols;
    var mat = useFloat32 ? new Float32Array(size) : new Float64Array(size);
    var pos = 0;
    for(var r = 0;r < rows;r += 1) {
      var row = arrs[r];
      if(row.length !== cols) {
        console.warn("invalid dimension in row " + r, row.length, cols);
        return null;
      }
      for(var c = 0;c < cols;c += 1) {
        mat[pos] = row[c];
        pos += 1;
      }
    }
    return new Matrix(mat, rows, cols);
  };
  this.eye = function(rows, c, useFloat32) {
    var cols = arguments.length < 2 ? rows : c;
    var size = rows * cols;
    if(rows <= 0 || cols <= 0) {
      console.warn("invalid dimensions", rows, cols);
      return null;
    }
    var mat = useFloat32 ? new Float32Array(size) : new Float64Array(size);
    var pos = 0;
    for(var i = 0;i < Math.min(rows, cols);i += 1) {
      mat[pos] = 1;
      pos += cols + 1;
    }
    return new Matrix(mat, rows, cols);
  };
  this.pivotRandom = function(m, k) {
    if(!m.isQuadratic()) {
      console.warn("quadratic matrix needed", m.rows(), m.cols());
      return null;
    }
    if(k < m.rows()) {
      console.warn("requested more pivots than elements", k, m.rows(), m.cols());
      return null;
    }
    var mat = m.createArray(k, m.cols());
    var pivots = {};
    var pos = 0;
    for(var i = 0;i < k;i += 1) {
      var pivot = 0;
      do {
        pivot = Math.random() * m.cols();
      } while(pivots[pivot]);
      pivots[pivot] = true;
      for(var c = 0;c < m.cols();c += 1) {
        mat[pos] = m.distance(pivot, c);
        pos += 1;
      }
    }
    return new Matrix(mat, k, m.cols());
  };

  Matrix=function (mat, rows, cols) {

    this.rows = function() {
      return rows;
    };
    this.cols = function() {
      return cols;
    };
    this.isQuadratic = function() {
      return rows === cols;
    };
    Matrix.prototype.noNaNs = function() {
      thatMDS.noNaNs(mat);
    };
    this.someRows = function(cb) {
      var pos = 0;
      for(var r = 0;r < rows;r += 1) {
        if(cb(mat.subarray(pos, pos + cols), r)) {
          return true;
        }
        pos += cols;
      }
      return false;
    };
    this.everyRows = function(cb) {
      return !this.someRows(function(row, ix) {
        return !cb(row, ix);
      });
    };
    this.rowsIter = function(cb) {
      var pos = 0;
      for(var r = 0;r < rows;r += 1) {
        cb(mat.subarray(pos, pos + cols), r);
        pos += cols;
      }
    };
    this.rowIter = function(row, cb) {
      var pos = row * cols;
      for(var i = 0;i < cols;i += 1) {
        cb(mat[pos], row, i);
        pos += 1;
      }
    };
    this.colIter = function(col, cb) {
      var pos = col;
      for(var i = 0;i < rows;i += 1) {
        cb(mat[pos], i, col);
        pos += cols;
      }
    };
    this.getUnsafe = function(pos) {
      return mat[pos];
    };
    this.createArray = function(rows, cols) {
      var size = rows * cols;
      return mat.byteLength > 24 ? new Float64Array(size) : new Float32Array(size);
    };
  }; // Matrix
  Matrix.prototype.toString = function() {
    var res = "";
    for(var r = 0;r < this.rows();r += 1) {
      this.rowIter(r, function(e, c) {
        res += " " + e;
      });
      res += "\n";
    }
    return res;
  };
  Matrix.prototype.iter = function(matB, row, col, cb) {
    Matrix.iter(this, matB, row, col, cb);
  };
  Matrix.prototype.mul = function(matB) {
    return Matrix.mul(this, matB);
  };
  Matrix.prototype.add = function(matB) {
    return Matrix.add(this, matB);
  };
  Matrix.prototype.neg = function() {
    var mat = this.createArray(this.rows(), this.cols());
    for(var pos = 0;pos < mat.length;pos += 1) {
      mat[pos] = -this.getUnsafe(pos);
    }
    return new Matrix(mat, this.rows(), this.cols());
  };
  Matrix.prototype.scale = function(scale) {
    var mat = this.createArray(this.rows(), this.cols());
    for(var pos = 0;pos < mat.length;pos += 1) {
      mat[pos] = scale * this.getUnsafe(pos);
    }
    return new Matrix(mat, this.rows(), this.cols());
  };
  Matrix.prototype.squareElements = function() {
    var mat = this.createArray(this.rows(), this.cols());
    for(var pos = 0;pos < mat.length;pos += 1) {
      mat[pos] = this.getUnsafe(pos) * this.getUnsafe(pos);
    }
    return new Matrix(mat, this.rows(), this.cols());
  };
  Matrix.prototype.colCenter = function() {
    var rows = this.rows();
    var cols = this.cols();
    var orig = this;
    var mat = this.createArray(rows, cols);
    for(var c = 0;c < cols;c += 1) {
      var avg = 0;
      orig.colIter(c, function(v) {
        avg += v;
      });
      avg /= rows;
      var pos = c;
      orig.colIter(c, function(v) {
        mat[pos] = v - avg;
        pos += cols;
      });
    }
    return new Matrix(mat, rows, cols);
  };
  Matrix.prototype.rowCenter = function() {
    var rows = this.rows();
    var cols = this.cols();
    var orig = this;
    var mat = this.createArray(rows, cols);
    var pos = 0;
    for(var r = 0;r < rows;r += 1) {
      var avg = 0;
      orig.rowIter(r, function(v) {
        avg += v;
      });
      avg /= cols;
      orig.rowIter(r, function(v) {
        mat[pos] = v - avg;
        pos += 1;
      });
    }
    return new Matrix(mat, rows, cols);
  };
  Matrix.prototype.doubleCenter = function() {
    var rows = this.rows();
    var cols = this.cols();
    var mat = this.createArray(rows, cols);
    for(var r = 0;r < rows;r += 1) {
      var avg = 0;
      this.rowIter(r, function(v) {
        avg += v;
      });
      avg /= cols;
      var pos = r * cols;
      this.rowIter(r, function(v) {
        mat[pos] = v - avg;
        pos += 1;
      });
    }
    for(var c = 0;c < cols;c += 1) {
      var avg = 0;
      var pos = c;
      for(var r = 0;r < rows;r += 1) {
        avg += mat[pos];
        pos += cols;
      }
      avg /= rows;
      pos = c;
      for(var r = 0;r < rows;r += 1) {
        mat[pos] -= avg;
        pos += cols;
      }
    }
    return new Matrix(mat, rows, cols);
  };
  Matrix.prototype.distance = function(colA, colB) {
    var res = 0;
    var posA = colA;
    var posB = colB;
    for(var r = 0;r < this.rows();r += 1) {
      var v = this.getUnsafe(posA) - this.getUnsafe(posB);
      res += v * v;
      posA += this.cols();
      posB += this.cols();
    }
    return Math.sqrt(res);
  };
  this.EIGEN_EPS = 1e-7;
  this.EIGEN_ITER = 10000;
  this.EIGEN_ITER_ASYNC = 200;
  Matrix.prototype.eigen = function(eigenVals) {
    var res;
    this.eigenAsync(eigenVals, function(mat) {
      res = mat;
    }, thatMDS.getCallDirect());
    return res;
  };
  Matrix.prototype.eigenAsync = function(eigenVals, cb, argCall) {
    var call = arguments.length > 2 ? argCall : thatMDS.CALL_ASYNC;
    var mat = this;
    var d = eigenVals.length;
    var rows = mat.rows();
    var cols = mat.cols();
    var content = mat.createArray(rows, cols);
    var pos = 0;
    for(var r = 0;r < rows;r += 1) {
      mat.rowIter(r, function(v) {
        content[pos] = v;
        pos += 1;
      });
    }
    var eigenVecs = mat.createArray(d, rows);
    var ePos = -rows;
    var m = 0;
    var r = 0;
    var iter = 0;

    innerLoop=function () {
      for(var ix = 0;ix < thatMDS.EIGEN_ITER_ASYNC;ix += 1) {
        if(!(Math.abs(1 - r) > thatMDS.EIGEN_EPS && iter < thatMDS.EIGEN_ITER)) {
          m += 1;
          iterate();
          return;
        }
        var q = mat.createArray(1, rows);
        pos = 0;
        for(var rix = 0;rix < rows;rix += 1) {
          for(var cix = 0;cix < cols;cix += 1) {
            q[rix] += content[pos] * eigenVecs[ePos + cix];
            pos += 1;
          }
        }
        eigenVals[m] = thatMDS.prod(eigenVecs, ePos, q, 0, rows);
        thatMDS.normalizeVec(q);
        r = Math.abs(thatMDS.prod(eigenVecs, ePos, q, 0, rows));
        thatMDS.xcopy(q, 0, eigenVecs, ePos, rows);
        iter += 1;
      }
      call(innerLoop);
    }; // innerLoop

    iterate=function () {
      if(!(m < d)) {
        cb(new Matrix(eigenVecs, d, rows));
        return;
      }

      if(m > 0) {
        pos = 0;
        for(var rix = 0;rix < rows;rix += 1) {
          for(var cix = 0;cix < cols;cix += 1) {
            content[pos] -= eigenVals[m - 1] * eigenVecs[ePos + rix] * eigenVecs[ePos + cix];
            pos += 1;
          }
        }
      }
      ePos += rows;
      pos = ePos;
      for(var i = 0;i < rows;i += 1) {
        eigenVecs[pos] = Math.random();
        pos += 1;
      }
      thatMDS.normalizeVec(eigenVecs, ePos, ePos + rows);
      r = 0;
      iter = 0;

      call(innerLoop);
    }; // iterate

    iterate();
  };
  Matrix.prototype.powerIter = function() {
    var res;
    this.powerIterAsync(function(r) {
      res = r;
    }, thatMDS.getCallDirect());
    return res;
  };
  Matrix.prototype.powerIterAsync = function(cb, argCall) {
    var call = arguments.length > 1 ? argCall : thatMDS.CALL_ASYNC;
    var mat = this;
    var rows = mat.rows();
    var cols = mat.cols();
    var r = mat.createArray(1, cols);
    for(var i = 0;i < cols;i += 1) {
      r[i] = Math.random();
    }
    var len = Number.POSITIVE_INFINITY;
    var stop = false;
    var iter = 0;

    iterate=function () {
      for(var ix = 0;ix < thatMDS.EIGEN_ITER_ASYNC;ix += 1) {
        if(iter >= thatMDS.EIGEN_ITER || stop) {
          cb(r);
          return;
        }
        var s = mat.createArray(1, cols);
        for(var row = 0;row < rows;row += 1) {
          var prod = 0;
          mat.rowIter(row, function(v, row, col) {
            prod += v * r[col];
          });
          mat.rowIter(row, function(v, row, col) {
            s[col] += prod * v;
          });
        }
        var nl = thatMDS.lengthSq(s);
        if(Math.abs(len - nl) < thatMDS.EIGEN_EPS) {
          stop = true;
        }
        len = nl;
        thatMDS.normalizeVec(s);
        r = s;
        iter += 1;
      }
      call(iterate);
    };

    iterate();
  };
  Matrix.iter = function(matA, matB, row, col, cb) {
    if(matA.cols() !== matB.rows()) {
      console.warn("incompatible dimensions", matA.rows() + "x" + matA.cols(), matB.rows() + "x" + matB.cols());
      return;
    }
    var posA = row * matA.cols();
    var posB = col;
    for(var i = 0;i < matA.cols();i += 1) {
      cb(matA.getUnsafe(posA), matB.getUnsafe(posB), row, i, col);
      posA += 1;
      posB += matB.cols();
    }
  };
  Matrix.mul = function(matA, matB) {
    if(matA.cols() !== matB.rows()) {
      console.warn("incompatible dimensions", matA.rows() + "x" + matA.cols(), matB.rows() + "x" + matB.cols());
      return null;
    }
    // cache friendly iteration (a rows -> a cols/b rows -> b cols)
    // TODO experiment with (a cols -> a rows -> b cols)
    var mat = matA.createArray(matA.rows(), matB.cols());
    for(var r = 0;r < matA.rows();r += 1) {
      matA.rowIter(r, function(a, _, k) {
        var pos = r * matB.cols();
        matB.rowIter(k, function(b, _, _) {
          mat[pos] += a * b;
          pos += 1;
        });
      });
    }
    return new Matrix(mat, matA.rows(), matB.cols());
  };
  Matrix.add = function(matA, matB) {
    if(matA.rows() !== matB.rows() || matA.cols() !== matB.cols()) {
      console.warn("incompatible dimensions", matA.rows() + "x" + matA.cols(), matB.rows() + "x" + matB.cols());
      return null;
    }
    var mat = matA.createArray(matA.rows(), matA.cols());
    for(var pos = 0;pos < mat.length;pos += 1) {
      mat[pos] = matA.getUnsafe(pos) + matB.getUnsafe(pos);
    }
    return new Matrix(mat, matA.rows(), matA.cols());
  };

}; // mdsjs

mdsjs = new mdsjs(); // create instance

